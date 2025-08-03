using MalbersAnimations.Reactions;
using MalbersAnimations.Scriptables;
using System.Linq;
using UnityEngine;

namespace MalbersAnimations
{
    [System.Serializable] //Needs to be Serializable!!!!
    [AddTypeMenu("Malbers/Scriptables/Set Bool Var Listener")]
    public class SetBoolVarReaction : Reaction
    {
        public override string DynamicName => $"Set Bool Var Listener [ID: {(ID.Value == -1 ? "Any" : ID.Value)}] to [{newValue.Value}]"; //Name of the Reaction

        public override System.Type ReactionType => typeof(BoolVarListener); //set the Type of component this Reaction Needs

        [Tooltip("ID for the Var Listener. If is set to -1 it will get the first Bool Listener found")]
        public IntReference ID = new(-1);
        public BoolReference newValue;


        protected override bool _TryReact(Component reactor)
        {
            var listenersP = reactor.GetComponentsInParent<BoolVarListener>().ToList();
            var listenersC = reactor.GetComponentsInChildren<BoolVarListener>().ToList();

            var mergeList = listenersP.Union(listenersC).ToList(); //Merge the two lists

            if (ID != -1)
            {
                mergeList = mergeList.FindAll(x => x.ID.Value == ID.Value); //Find all in Parent
            }

            if (mergeList != null)
            {
                foreach (var item in mergeList)
                {
                    item.Value = (newValue.Value);
                }
                return true; //Reaction successful!!
            }

            return false;
        }
    }
}