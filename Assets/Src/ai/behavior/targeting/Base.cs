namespace ai.behavior.targeting
{
    public class Base
    {
        public virtual void Update()
        {
            //do nothing
        }

        public virtual bool IsFinished
        {
            get { return false; }
        }
    }
}
