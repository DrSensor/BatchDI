using Microsoft.AspNetCore.Mvc;

namespace Example.Controllers
{
    using Services;

    public class DummyController : Controller
    {
        private SingletonExService exsingleton;
        private SingletonService singleton;
        // private ITransientService transient;
        // private ScopedService scoped;

        public DummyController(
            // TransientService transient,
            // ScopedService scoped,
            SingletonService singleton,
            SingletonExService exsingleton
            )
        {
            this.exsingleton = exsingleton;
            this.singleton = singleton;
            // this.transient = transient;
            // this.scoped = scoped;
        }

        public IActionResult Print()
        {
            return Ok(new
            {
                ExSingleton = exsingleton.count,
                Singleton = singleton.count,
                // Transient = transient.count,
                // Scoped = scoped.count
            });
        }
    }
}