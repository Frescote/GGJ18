﻿using Assets.Scripts.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.Scripts
{
    public class PlayerInfo : MonoBehaviour
    {
        public string Sequence { get; set; }
        public string Stack { get; set; }

        public string CurrentState;

        public Sprite StateG;
        public Sprite StateT;
        public Sprite StateA;
        public Sprite StateC;

        private System.Random _random_generator;

        public void SetUp()
        {
            Sequence = "";
            Stack = "";
            CurrentState = "";
            _random_generator = new System.Random();

            for (int i = 0; i < PlayerConsts.SEQUENCE_NUMBER; i++) { 
                Sequence += PlayerConsts.SEQUENCE_STATES[UnityEngine.Random.Range(0, PlayerConsts.SEQUENCE_STATES.Length)];
            }

            CurrentState = PlayerConsts.SEQUENCE_STATES[UnityEngine.Random.Range(0, PlayerConsts.SEQUENCE_STATES.Length)];
            ChangeSprite();
        }

        void ChangeSprite()
        {
            switch (CurrentState)
            {
                case "A":
                    this.GetComponent<SpriteRenderer>().sprite = StateA;
                    break;
                case "T":
                    this.GetComponent<SpriteRenderer>().sprite = StateT;
                    break;
                case "G":
                    this.GetComponent<SpriteRenderer>().sprite = StateG;
                    break;
                case "C":
                    this.GetComponent<SpriteRenderer>().sprite = StateC;
                    break;
            }
        }

        public void ReceiveState(string state)
        {
            CurrentState = state;
            ChangeSprite();
            Debug.Log("CurrentState: " + CurrentState + " Sequence " + Sequence + " Sequence[Stack.Length] " + Sequence[Stack.Length]);
            if (Sequence[Stack.Length].Equals(state[0]))
            {
                Stack += state;
                if (Sequence.Equals(Stack))
                {
                    GameObject.FindGameObjectWithTag("WinPanel").SetActive(true);

                    //PhotonNetwork.LeaveRoom();
                    //SceneManager.LoadScene(0);
                }
            }
            else
            {
                Stack = "";
            }
        }

        private void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
        {
            if (stream.isWriting)
            {
                stream.SendNext(Sequence);
                stream.SendNext(Stack);
                stream.SendNext(CurrentState);
                //Debug.Log("Writing!!!");
            }
            else 
            {
                Sequence = (string)stream.ReceiveNext();
                Stack = (string)stream.ReceiveNext();
                CurrentState = (string)stream.ReceiveNext();
                //Debug.Log(((string)stream.ReceiveNext())[0]);
            }
        }

    }
}
