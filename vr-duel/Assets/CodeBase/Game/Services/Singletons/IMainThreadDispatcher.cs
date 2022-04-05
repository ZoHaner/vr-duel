using System;
using System.Collections;
using System.Threading.Tasks;
using CodeBase.ServiceLocator;

namespace CodeBase.Services.Singletons
{
    public interface IMainThreadDispatcher : IService
    {
        /// <summary>
        /// Locks the queue and adds the IEnumerator to the queue
        /// </summary>
        /// <param name="action">IEnumerator function that will be executed from the main thread.</param>
        void Enqueue(IEnumerator action);

        /// <summary>
        /// Locks the queue and adds the Action to the queue
        /// </summary>
        /// <param name="action">function that will be executed from the main thread.</param>
        void Enqueue(Action action);

        /// <summary>
        /// Locks the queue and adds the Action to the queue, returning a Task which is completed when the action completes
        /// </summary>
        /// <param name="action">function that will be executed from the main thread.</param>
        /// <returns>A Task that can be awaited until the action completes</returns>
        Task EnqueueAsync(Action action);
    }
}