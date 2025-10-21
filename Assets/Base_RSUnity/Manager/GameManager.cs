using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Wigi.Base
{
    public class GameManager : Singleton<GameManager>
    {
        public static void QuitGame()
        {
            Application.Quit();
        }

        #region  method OnGame
        void OnStartGame()
        {
            Debug.Log("__________Start Game__________");
        }

        //show keyboard: false,  start game: true, notift: false, menu: false
        void OnFocusGame(bool focus)
        {
            Debug.Log("__________Focus Game__________");
        }

        //click home: true
        void OnPauseGame(bool pause)
        {
            Debug.Log("__________Pause Game__________");
        }

        //when call quit game
        void OnQuitGame()
        {
            Debug.Log("__________Quit Game__________");
        }

        //after call quit game, game is destroy
        void OnDestroyObjectGame()
        {

        }
        #endregion

        #region method unity
        protected override void Awake()
        {
            base.Awake();
            OnStartGame();
        }

        private void OnApplicationFocus(bool focus)
        {
            OnFocusGame(focus);
        }

        private void OnApplicationPause(bool pause)
        {
            OnPauseGame(pause);
        }

        private void OnApplicationQuit()
        {
            OnQuitGame();
        }

        public override void OnDestroy()
        {
            base.OnDestroy();
            OnDestroyObjectGame();
        }

#endregion        
    }
}
