using System;
using System.Threading.Tasks;
using GameCreator.Runtime.Common;
using UnityEngine;
using GameCreator.Runtime.Characters;

namespace GameCreator.Runtime.VisualScripting
{
    [Version(1, 0, 0)]

    [Title("Mount Character")]
    [Description("Disable collision detection for Character")]

    [Category("Characters/Animation/Mount Character")]

    [Parameter("Character", "Target character for disable collision detection")]

    [Keywords("Mount", "Player", "Characters", "Collision")]
    [Image(typeof(IconCharacterState), ColorTheme.Type.Red)]
    
    [Serializable]
    public class InstructionCharacterMount : Instruction
    {
        [SerializeField] private PropertyGetGameObject m_Character = GetGameObjectPlayer.Create();

        public override string Title => string.Format("Mount {0}",this.m_Character);

        protected override Task Run(Args args)
        {
            Character character = this.m_Character.Get<Character>(args);
            if (character == null) return DefaultResult;
            
            CharacterController controller = character.GetComponent<CharacterController>();
            if (controller == null) return DefaultResult;

            controller.detectCollisions = false;
            controller.enabled = false;

            return DefaultResult;
        }
    }
}