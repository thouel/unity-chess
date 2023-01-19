using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace TH {
    public class Board: MonoBehaviour {

        private GameManager gameManager;

        #region Configuration
        [Header("Cells Cconfiguration")]
        public Transform cellsParent;
        public GameObject cellPrefab;
        public Color lightColor = new(0.87f, 0.8f, 1f, 0.8f);
        public Color darkColor = new(0.23f, 0f, 0.41f, 0.8f);
        public Color selectColor = new(1.0f, 1.0f, 0.8f, 1f);
        public Color threatColor = new(1.0f, 0.15f, 0.15f, 0.5f);

        [Header("Pieces Configuration")]
        public Transform piecesParent;

        [Header("Light Prefabs")]
        public GameObject lightPawnPrefab;
        public GameObject lightRookPrefab;
        public GameObject lightKnightPrefab;
        public GameObject lightBishopPrefab;
        public GameObject lightKingPrefab;
        public GameObject lightQueenPrefab;

        [Header("Dark Prefabs")]
        public GameObject darkPawnPrefab;
        public GameObject darkRookPrefab;
        public GameObject darkKnightPrefab;
        public GameObject darkBishopPrefab;
        public GameObject darkKingPrefab;
        public GameObject darkQueenPrefab;

        [Header("Removed Pieces Configuration")]
        public Transform removedDarkPieces;
        public Transform removedLightPieces;
        #endregion

        #region Actual Game Data
        [Header("Actual Game Data")]
        [SerializeField]
        public Cell[,] cells = new Cell[8, 8];
        // Store the pieces with their current position in board {col}{row} as key
        [SerializeField]
        public Dictionary<string, Piece> pieces = new Dictionary<string, Piece>();
        [SerializeField]
        public List<Piece> removedPieces = new List<Piece>();
        [SerializeField]
        public List<Cell> availableCells = new();
        [SerializeField]
        public King lightKing;
        [SerializeField]
        public King darkKing;
        #endregion

        private void Awake() {
            gameManager = FindObjectOfType<GameManager>();
        }

        #region Handle Play Actions
        public void HandleSelection(int col, int row, bool lightTurn) {
            Piece piece = GetPieceAt(col, row);
            if (piece != null) {
                // Check to determine if it's a piece of the right side that has been selected
                if (piece.isLight != lightTurn) {
                    gameManager.SetNotifier($"Not your turn ! Can not move the piece {piece.name} from [{col}, {row}]");
                    return;
                }

                // Retrieve available moves depending on the current board
                availableCells = piece.GetAvailableCells();

                // TODO: If there's a checkmate ongoing, need to :
                // - mark all threats pieces
                // - remove the forbidden cells from available
                // - test if there is any available cells except from the piece's one
                //   - if not, checkmate => endgame
                //   - if there is, let the move go as others
                if (gameManager.controlCheckMate) {
                    bool checkOngoing = IsThereACheckOngoing();
                    if (checkOngoing) {
                        King king = lightTurn ? lightKing : darkKing;
                        foreach (Piece p in king.threats) {
                            Cell c = GetCell(p);
                            c.EnableThreatColor();
                        }

                        foreach (Cell c in king.forbiddenCells) {
                            if (availableCells.Contains(c)) {
                                if (piece.IsKing()) {
                                    availableCells.Remove(c);
                                    c.EnableThreatColor();
                                }
                            }
                        }
                    }
                }

                // Add the selected cell to the list
                availableCells.Add(cells[col, row]);

                // Highlight the available cells
                foreach (Cell c in availableCells) {
                    c.EnableSelectColor();
                }
            }
        }

        public void HandleUnselection(bool lightTurn) {
            foreach (Cell c in availableCells) {
                c.DisableSelectColor();
            }
            if (gameManager.controlCheckMate) {
                King king = lightTurn ? lightKing : darkKing;
                foreach (Piece p in king.threats) {
                    Cell c = GetCell(p);
                    c.DisableThreatColor();
                }
                foreach (Cell c in king.forbiddenCells) {
                    c.DisableThreatColor();
                }
            }
            availableCells.Clear();
        }

        public void HandleClick(int col, int row, bool lightTurn) {
            if (col < 0 || col > 7 || row < 0 || row > 7) {
                HandleUnselection(lightTurn);
                return;
            }

            Cell cell = cells[col, row];

            // Test if the click has been made on :
            //   a cell not available to move the piece
            //   the piece itself
            if (!availableCells.Contains(cell) || availableCells.IndexOf(cell) == availableCells.Count - 1) {
                HandleUnselection(lightTurn);
                return;
            } else {
                HandleMove(col, row, lightTurn);
            }
        }

        public void HandleMove(int col, int row, bool lightTurn) {
            Cell originCell = availableCells[availableCells.Count - 1];
            Piece piece = GetPieceAt(originCell);
            Piece pieceAtDestination = GetPieceAt(col, row);

            if (piece.isLight != lightTurn) {
                gameManager.SetNotifier($"Not your turn ! Can not move the piece {piece.name} to [{col}, {row}]");
                return;
            }
            
            if (pieceAtDestination != null) {
                // Eat the actual piece if there is one
                RemovePieceAtLocation(col, row);
            }

            // Update the board
            UpdatePieceLocation(piece.col, piece.row, col, row);

            // Update piece
            piece.SetBoardPos(col, row);

            // Move the actuel piece
            piece.Move(col, row);

            // Update Game Manager
            gameManager.UpdateStatesAfterMove();

            // Unselect everything at turn's end
            HandleUnselection(lightTurn);
        }
        
        private bool IsThereACheckOngoing() {
            King king = null;
            if (gameManager.lightTurn) {
                king = lightKing;
            } else {
                king = darkKing;
            }

            king.UpdateThreats();

            return king.IsThreatened();
        }
        #endregion

        #region Setup the Board
        public void SetupCells() {
            for (int i = 0; i < 8; i++) {
                for (int j = 0; j < 8; j++) {
                    // Calculate the case color
                    Color color = ((i + j) % 2 == 0 ? darkColor : lightColor);

                    // Calculate the coordinates
                    Vector2Int playPosition = new Vector2Int(i, j);
                    Vector2 pos = BoardPosToScreenPos(playPosition);

                    // Instantiate the GameObject
                    GameObject cellObject = Instantiate(cellPrefab, pos, Quaternion.identity, cellsParent);

                    // Find the Cell Object to keep a reference to it
                    cells[i, j] = cellObject.GetComponent<Cell>();

                    // Setup the cell
                    cells[i, j].Setup(playPosition, cellObject, color, selectColor, threatColor, this);
                }
            }
        }

        public void SetupPieces() {

            /*** Place pieces
             * We have nothing to place on rows 2, 3, 4, 5
             * We only place on rows 0, 1 for the dark side
             * And on rows 6, 7 for the light side
             * For rows 1 & 6, it's only Pawns
             * For rows 0 & 7, it's the Special Pieces :
             *   0 & 7 = Rook
             *   1 & 6 = Knight
             *   2 & 5 = Bishop
             *   3 = Queen
             *   4 = King
             */
            for (int i = 0; i < 8; i++) {
                SetupPiece(darkPawnPrefab, i, 1);
            }
            SetupPiece(darkRookPrefab, 0, 0);
            SetupPiece(darkRookPrefab, 7, 0);
            SetupPiece(darkKnightPrefab, 1, 0);
            SetupPiece(darkKnightPrefab, 6, 0);
            SetupPiece(darkBishopPrefab, 2, 0);
            SetupPiece(darkBishopPrefab, 5, 0);
            SetupPiece(darkQueenPrefab, 3, 0);
            SetupPiece(darkKingPrefab, 4, 0);

            for (int i = 0; i < 8; i++) {
                SetupPiece(lightPawnPrefab, i, 6);
            }
            SetupPiece(lightRookPrefab, 0, 7);
            SetupPiece(lightRookPrefab, 7, 7);
            SetupPiece(lightKnightPrefab, 1, 7);
            SetupPiece(lightKnightPrefab, 6, 7);
            SetupPiece(lightBishopPrefab, 2, 7);
            SetupPiece(lightBishopPrefab, 5, 7);
            SetupPiece(lightQueenPrefab, 3, 7);
            SetupPiece(lightKingPrefab, 4, 7);

            UpdateLightKing();
            UpdateDarkKing();
        }

        private void SetupPiece(GameObject piecePrefab, int col, int row) {
            GameObject gameObject = Instantiate(piecePrefab, cells[col, row].transform.position, Quaternion.identity, piecesParent);
            gameObject.name = $"{piecePrefab.name} {col}{row}";
            Piece piece = gameObject.GetComponent<Piece>();
            piece.SetBoardPos(col, row);
            pieces.Add($"{col}{row}", piece);
        }
        #endregion

        #region Tools
        private void RemovePieceAtLocation(int col, int row) {
            Piece piece = GetPieceAt(col, row);
            
            // Keep a reference to the object in case we need to
            // spawn it later in the game
            removedPieces.Add(piece);
            
            if (piece.isLight) {
                // Move the piece outside of the board
                piece.BeEaten(removedLightPieces.transform);
            } else {

                // Move the piece outside of the board
                piece.BeEaten(removedDarkPieces.transform);
            }

            // Remove the piece from the Dictionary
            pieces.Remove($"{col}{row}");
        }

        private void UpdatePieceLocation(int oldCol, int oldRow, int newCol, int newRow) {
            Piece piece = GetPieceAt(oldCol, oldRow);
            
            // Remove the piece at his old position in the Dictionary
            pieces.Remove($"{oldCol}{oldRow}");

            // Add the piece at his new position in the Dictionary
            pieces.Add($"{newCol}{newRow}", piece);
        }
        
        public Piece GetPieceAt(int col, int row) {
            Piece piece = null;
            pieces.TryGetValue($"{col}{row}", out piece);
            return piece;
        }

        public Piece GetPieceAt(Vector2Int v) {
            return GetPieceAt(v.x, v.y);
        }

        public Piece GetPieceAt(Cell c) {
            return GetPieceAt(c.col, c.row);
        }

        public Piece GetPieceAtScreenPos(Vector2 pos) {
            return GetPieceAt(ScreenPosToBoardPos(pos));
        }

        public static Vector2 BoardPosToScreenPos(Vector2Int boardPosition) {
            return new Vector2((boardPosition.x * 100) + 50, (boardPosition.y * 100) + 50);
        }

        public static Vector2Int ScreenPosToBoardPos(Vector2 screenPos) {
            Vector2Int v = new Vector2Int();
            v.x = (int) screenPos.x / 100;
            v.y = (int) screenPos.y / 100;
            return v;
        }
    
        public Cell GetOneCellDown(int col, int row) {
            int r = row - 1;
            return GetCell(col, r);
        }

        public Cell GetOneCellUp(int col, int row) {
            int r = row + 1;
            return GetCell(col, r);
        }

        public Cell GetCell(Piece p) {
            return GetCell(p.col, p.row);
        }

        public Cell GetCell(int col, int row) {
            if (col < 0 || col > 7) return null;
            if (row < 0 || row > 7) return null;
            return cells[col, row];
        }
        
        private void UpdateLightKing() {
            foreach (Piece p in pieces.Values) {
                if (p.role.Equals(Piece.Role.LightKing)) {
                    lightKing = (King) p;
                    return;
                }
            }
        }

        private void UpdateDarkKing() {
            foreach (Piece p in pieces.Values) {
                if (p.role.Equals(Piece.Role.DarkKing)) {
                    darkKing = (King) p;
                    return;
                }
            }
        }
        #endregion
    }
}
