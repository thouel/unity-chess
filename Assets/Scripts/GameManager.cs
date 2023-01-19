using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.Camera;

namespace TH {
    public class GameManager : MonoBehaviour {

        public Board board;
        public Camera myCamera;

        public GameObject notifierUI;
        public TextMeshProUGUI notifierText;

        [Header("Inputs")]
        public bool leftclick_input = false;
        public Vector3 leftClickPosition = Vector3.zero;

        [Header("Game Status")]
        [SerializeField]
        public bool lightTurn = true;
        public bool pieceSelectFlag = false;
        public int moves = 0;
        public bool controlCheckMate = false;

        [Header("Countdown Configuration")]
        public bool firstClickMade = false;
        public float initialCounter = 180f;
        public float lightCounter;
        public float darkCounter;
        public TextMeshProUGUI lightCountdownText;
        public TextMeshProUGUI darkCountdownText;

        private void Awake() {
            myCamera = FindObjectOfType<Camera>();
        }

        private void Start() {

            // Draw cells empty
            board.SetupCells();

            // Add Pieces
            board.SetupPieces();

            // Setup Stopwatches
            lightCounter = initialCounter;
            darkCounter = initialCounter;
            lightCountdownText.text = GetStopwatchValue(lightCounter);
            darkCountdownText.text = GetStopwatchValue(darkCounter);
        }
        private void Update() {
            TickInput();

            float delta = Time.deltaTime;
            UpdateCountdownUI(delta);

            if (lightCounter <= 0 || darkCounter <= 0) {
                Debug.Log($"Finished {lightCounter}, {darkCounter}");
                SetNotifier($"Finished {lightCounter}, {darkCounter}");
                // TODO: Handle the endgame properly
            }

            if (leftclick_input) {

                if (!firstClickMade) {
                    firstClickMade = true;
                }

                Vector2Int v = Board.ScreenPosToBoardPos(leftClickPosition);
                int col = v.x;
                int row = v.y;

                if (pieceSelectFlag == false) {
                    board.HandleSelection(col, row, lightTurn);
                    pieceSelectFlag = board.availableCells.Count > 0 ? true : false;
                } else {
                    DisableNotifier();
                    board.HandleClick(col, row, lightTurn);
                    pieceSelectFlag = false;
                }
            }
        }
        private void TickInput() {
            leftclick_input = Input.GetMouseButtonDown(0);
            if (leftclick_input) {
                leftClickPosition = myCamera.ScreenToWorldPoint(Input.mousePosition);
            }
        }
        private string GetStopwatchValue(float counter) {
            int minutes = Mathf.FloorToInt(counter / 60);
            int seconds = Mathf.FloorToInt(counter % 60);
            int milliseconds = Mathf.FloorToInt((counter % 1) * 1000);
            return $"{minutes:00} : {seconds:00}, {milliseconds:000}";
        }
        private void UpdateCountdownUI(float delta) {
            if (lightTurn) {
                if (firstClickMade) {
                    lightCounter -= delta;
                }
                lightCountdownText.text = GetStopwatchValue(lightCounter);
            } else {
                if (firstClickMade) {
                    darkCounter -= delta;
                }
                darkCountdownText.text = GetStopwatchValue(darkCounter);
            }
        }
        public void UpdateStatesAfterMove() {
            moves++;
            lightTurn = !lightTurn;
        }
        public void SetNotifier(string text) {
            notifierText.text = text;
            notifierUI.SetActive(true);
        }
        public void DisableNotifier() {
            notifierUI.SetActive(false);
        }
    }
}
