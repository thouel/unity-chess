using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

namespace TH {
    public class Cell : MonoBehaviour {
        public GameObject cellObject;
        public SpriteRenderer notSelectedComponent;
        public SpriteRenderer selectedComponent;
        public SpriteRenderer threatComponent;
        public Color cellColor;
        public Color selectColor;
        public Color threatColor;
        public Board board;
        public int col;
        public int row;

        public void Setup(Vector2Int position, GameObject gameObject, Color cellColor, Color selectColor, Color threatColor, Board board) {
            this.cellObject = gameObject;
            this.cellColor = cellColor;
            this.threatColor = threatColor;
            this.board = board;

            col = position.x;
            row = position.y;

            SpriteRenderer[] srs = cellObject.GetComponentsInChildren<SpriteRenderer>();
            
            foreach (SpriteRenderer sr in srs) {
                if (sr.name.Equals("Selected")) {
                    sr.color = selectColor;
                    selectedComponent = sr;
                    selectedComponent.enabled = false;
                } else if (sr.name.Equals("Threat")) {
                    sr.color = threatColor;
                    threatComponent = sr;
                    threatComponent.enabled = false;
                } else if (sr.name.Equals("Not Selected")) {
                    sr.color = cellColor;
                    notSelectedComponent = sr;
                }
            }

            // Name it nicely in the Project Explorer
            this.name = $"{col}{row}";
        }

        public void EnableThreatColor() {
            threatComponent.enabled = true;
            selectedComponent.enabled = false;
            notSelectedComponent.enabled = false;
        }

        public void DisableThreatColor() {
            threatComponent.enabled = false;
            selectedComponent.enabled = false;
            notSelectedComponent.enabled = true;
        }

        public void EnableSelectColor() {
            threatComponent.enabled = false;
            selectedComponent.enabled = true;
            notSelectedComponent.enabled = false;
        }

        public void DisableSelectColor() {
            threatComponent.enabled = false;
            selectedComponent.enabled = false;
            notSelectedComponent.enabled = true;
        }
    }
}
