using UnityEngine;
using Wigi.StateMachine;

namespace Wigi.Turorial
{
    public enum TutorialState
    {
        //Comic
        StartComic,

        //GamePlay
        StartPlay,
        JoystickPlay,

        PrisonPlay, PrisonRelease,
        TreeBuffMove, TreeBuffChoose,
        EliteBoss,
        Reward,

        //Lobby
        LobbyHero, UpgradeHero,
        LobbyCaptain, EnhanceCaptain,
        LobbyTroop, BuyTroop,
        LobbyFight
    }

    public class TutorialPlayer : MonoBehaviour, IState<TutorialState>
    {


        StateMachine<TutorialState> stateMachine;

        // Start is called before the first frame update
        void Start()
        {
            stateMachine = new StateMachine<TutorialState>(this);
        }

        // Update is called once per frame
        void Update()
        {
            stateMachine.Update(Time.deltaTime);
        }



        public void OnEnterState(TutorialState state)
        {
            throw new System.NotImplementedException();
        }

        public void OnExitState(TutorialState state)
        {
            throw new System.NotImplementedException();
        }

        public void UpdateState(float dt)
        {
            throw new System.NotImplementedException();
        }
    }
}
