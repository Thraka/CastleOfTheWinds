namespace CastleOfTheWinds
{
    internal class AttackAction : GameAction
    {
        private readonly Creature _attacker;
        private readonly Creature _target;

        public AttackAction(Creature attacker, Creature target)
        {
            _attacker = attacker;
            _target = target;
        }

        public override int Duration => 2;

        public override bool Repeat => false;

        public override bool StartAction()
        {
            throw new System.NotImplementedException();
        }

        public override void CompleteAction()
        {
        }
    }
}