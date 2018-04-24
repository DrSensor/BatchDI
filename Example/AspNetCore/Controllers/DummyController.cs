using Microsoft.AspNetCore.Mvc;

namespace Example.Controllers
{
    using Services;

    public class DummyController : Controller
    {
        private SingletonService singleton;
        private ITransientService transient;
        private ScopedService scoped;

        public DummyController(SingletonService singleton, TransientService transient, ScopedService scoped)
        {
            this.singleton = singleton;
            this.transient = transient;
            this.scoped = scoped;
        }

        public IActionResult Print()
        {
            return Ok(new
            {
                Singleton = singleton.count,
                Transient = transient.count,
                Scoped = scoped.count
            });
        }
    }
}