using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace TH {
    public abstract class Piece: MonoBehaviour {

        protected Board board;

        [Header("Position in board")]
        [SerializeField]
        public int col;
        [SerializeField]
        public int row;
        [SerializeField]
        public bool isMoving = false;
        public Vector2 movingDestination = Vector2.zero;
        public Transform newParent = null;

        #region Configuration
        public float moveSpeed = 4.0f;

        [Header("Identification")]
        public bool isLight;
        public Role role;

        public enum Role {
            LightPawn,
            LightRook,
            LightKnight,
            LightBishop,
            LightKing,
            LightQueen,
            DarkPawn,
            DarkRook,
            DarkKnight,
            DarkBishop,
            DarkKing,
            DarkQueen,
        }
        #endregion

        protected void Awake() {
            board = FindObjectOfType<Board>();
        }

        public void SetBoardPos(int col, int row) {
            this.col = col;
            this.row = row;
        }

        #region General Actions
        public abstract List<Cell> GetAvailableCells();

        public virtual void Move(int col, int row) {
            movingDestination = Board.BoardPosToScreenPos(new Vector2Int(col, row));
            isMoving = true;
        }

        public void BeEaten(Transform transform) {
            movingDestination = transform.position;
            newParent = transform;
            isMoving = true;
        }
        #endregion

        private void FixedUpdate() {
            float delta = Time.fixedDeltaTime;

            if (isMoving) {
                // Lerp the piece to the position expected
                transform.position = Vector2.Lerp(transform.position, movingDestination, moveSpeed * delta);

                float distance = Vector2.Distance(transform.position, movingDestination);

                if (distance < 2.0f) {
                    transform.position = movingDestination;
                    if (newParent != null) {
                        transform.SetParent(newParent, false);
                        newParent = null;
                    }
                    isMoving = false;
                    movingDestination = Vector2.zero;
                }
            }
        }

        protected bool IsOneOfMine(Piece piece) {
            return isLight == piece.isLight;
        }

        public bool IsKing() {
            return role.Equals(Role.LightKing) || role.Equals(Role.DarkKing);
        }

        public bool IsKnight() {
            return role.Equals(Role.LightKnight) || role.Equals(Role.DarkKnight);
        }

        #region Search Up, Down, Left, Right
        protected List<Cell> SearchUp() {
            List<Cell> cells = new List<Cell>();

            if (row + 1 > 7) {
                // We are on top of the board, nothing to add
                return cells;
            }

            for (int i = row + 1; i < 8; i++) {
                Piece p = board.GetPieceAt(col, i);
                if (p == null) {
                    // The cell is unoccupied, we can add it
                    cells.Add(board.GetCell(col, i));
                } else {
                    if (!IsOneOfMine(p)) {
                        // The cell is occupied by an opponent piece,
                        // we can add it to the available moves
                        cells.Add(board.GetCell(col, i));
                    }
                    // And we need to break the loop as there is
                    // a piece blocking this way
                    break;
                }
            }

            return cells;
        }

        protected List<Cell> SearchDown() {
            List<Cell> cells = new List<Cell>();

            if (row - 1 < 0) {
                // We are on bottom of the board, nothing to add
                return cells;
            }
            for (int i = row - 1; i >= 0; i--) {
                Piece p = board.GetPieceAt(col, i);
                if (p == null) {
                    // The cell is unoccupied, we can add it
                    cells.Add(board.GetCell(col, i));
                } else {
                    if (!IsOneOfMine(p)) {
                        // The cell is occupied by an opponent piece,
                        // we can add it to the available moves
                        cells.Add(board.GetCell(col, i));
                    }
                    // And we need to break the loop as there is
                    // a piece blocking the way
                    break;
                }
            }

            return cells;
        }

        protected List<Cell> SearchLeft() {
            List<Cell> cells = new List<Cell>();

            if (col - 1 < 0) {
                // We are on the left of the board, nothing to add
                return cells;
            }
            for (int i = col - 1; i >= 0; i--) {
                Piece p = board.GetPieceAt(i, row);
                if (p == null) {
                    // The cell is unoccupied, we can add it
                    cells.Add(board.GetCell(i, row));
                } else {
                    if (!IsOneOfMine(p)) {
                        // The cell is occupied by an opponent piece,
                        // we can add it to the available moves
                        cells.Add(board.GetCell(i, row));
                    }
                    // And we need to break the loop as there is
                    // a piece blocking the way
                    break;
                }
            }

            return cells;
        }

        protected List<Cell> SearchRight() {
            List<Cell> cells = new List<Cell>();

            if (col + 1 > 7) {
                // We are on top of the board, nothing to add
                return cells;
            }
            for (int i = col + 1; i < 8; i++) {
                Piece p = board.GetPieceAt(i, row);
                if (p == null) {
                    // The cell is unoccupied, we can add it
                    cells.Add(board.GetCell(i, row));
                } else {
                    if (!IsOneOfMine(p)) {
                        // The cell is occupied by an opponent piece,
                        // we can add it to the available moves
                        cells.Add(board.GetCell(i, row));
                    }
                    // And we need to break the loop as there is
                    // a piece blocking this way
                    break;
                }
            }

            return cells;
        }
        #endregion
        #region Search Diagonal Up-Left, Up-Right, Down-Left, Down-Right
        protected List<Cell> SearchDiagonalUpLeft() {
            List<Cell> cells = new List<Cell>();

            if (row + 1 > 7) {
                return cells;
            }
            if (col - 1 < 0) {
                return cells;
            }

            for (int i = col - 1, j = row + 1; i >= 0 && j < 8; i--, j++) {
                Piece p = board.GetPieceAt(i, j);
                if (p == null) {
                    cells.Add(board.GetCell(i, j));
                } else {
                    if (!IsOneOfMine(p)) {
                        cells.Add(board.GetCell(i, j));
                    }
                    break;
                }
            }

            return cells;
        }

        protected List<Cell> SearchDiagonalUpRight() {
            List<Cell> cells = new List<Cell>();

            if (row + 1 > 7) {
                // We are on top of the board, nothing to add
                return cells;
            }
            if (col + 1 > 7) {
                // We are on top of the board, nothing to add
                return cells;
            }

            for (int i = col + 1, j = row + 1; i < 8 && j < 8; i++, j++) {
                Piece p = board.GetPieceAt(i, j);
                if (p == null) {
                    cells.Add(board.GetCell(i, j));
                } else {
                    if (!IsOneOfMine(p)) {
                        cells.Add(board.GetCell(i, j));
                    }
                    break;
                }
            }

            return cells;
        }

        protected List<Cell> SearchDiagonalDownLeft() {
            List<Cell> cells = new List<Cell>();

            if (row - 1 < 0) {
                return cells;
            }
            if (col - 1 < 0) {
                return cells;
            }

            for (int i = col - 1, j = row - 1; i >= 0 && j >= 0; i--, j--) {
                Piece p = board.GetPieceAt(i, j);
                if (p == null) {
                    cells.Add(board.GetCell(i, j));
                } else {
                    if (!IsOneOfMine(p)) {
                        cells.Add(board.GetCell(i, j));
                    }
                    break;
                }
            }

            return cells;
        }

        protected List<Cell> SearchDiagonalDownRight() {
            List<Cell> cells = new List<Cell>();

            if (row - 1 < 0) {
                return cells;
            }
            if (col + 1 > 7) {
                return cells;
            }

            for (int i = col + 1, j = row - 1; i < 8 && j >= 0; i++, j--) {
                Piece p = board.GetPieceAt(i, j);
                if (p == null) {
                    cells.Add(board.GetCell(i, j));
                } else {
                    if (!IsOneOfMine(p)) {
                        cells.Add(board.GetCell(i, j));
                    }
                    break;
                }
            }

            return cells;
        }
        #endregion
    }
}
