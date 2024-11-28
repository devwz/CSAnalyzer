namespace App2
{
    public class App
    {
        public void Run()
        {
            DoSomething doSomething = new DoSomething();
            doSomething.Logic1();

            Log log = new Log();
            log.LogInfo();
        }
    }
}
