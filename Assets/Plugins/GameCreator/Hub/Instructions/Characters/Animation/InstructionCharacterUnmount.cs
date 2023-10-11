using System;
using System.Threading.Tasks;
using GameCreator.Runtime.Common;
using UnityEngine;
using GameCreator.Runtime.Characters;

namespace GameCreator.Runtime.VisualScripting
{
    [Version(1, 0, 0)]

    [Title("Unmount Character")]
    [Description("Enable collision detection for Character")]

    [Category("Characters/Animation/Unmount Character")]

    [Parameter("Character", "Target character for enable collision detection")]

    [Keywords("Unmount", "Mount", "Player", "Characters", "Collision")]
    [Image(typeof(IconCharacterState), ColorTheme.Type.Blue)]
    
    [Serializable]
    public class InstructionCharacterUnmount : Instruction
    {
        [SerializeField] private PropertyGetGameObject m_Character = GetGameObjectPlayer.Create();

        public override string Title => string.Format("Unmount {0}",this.m_Character);

        protected override Task Run(Args args)
        {
            Character character = this.m_Character.Get<Character>(args);
            if (character == null) return DefaultResult;
            
            CharacterController controller = character.GetComponent<CharacterController>();
            if (controller == null) return DefaultResult;

            controller.detectCollisions = true;
            controller.enabled = true;

            return DefaultResult;
        }
    }
}