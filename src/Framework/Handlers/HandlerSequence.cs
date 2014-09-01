namespace Lacjam.Framework.Handlers
{
    public class HandlerSequence
    {
        protected HandlerSequence()
        {
            
        }
        public HandlerSequence(string name)
        {
            Name = name;
            Seq = 0;
        }

        public virtual void UpdateSequence(long seq)
        {
            if (seq <= Seq) 
                return;

            Seq = seq;
        }

        public virtual string Name { get; protected set; }
        public virtual long Seq { get; protected set; }
    }
}