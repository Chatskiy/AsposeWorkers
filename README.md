# AsposeWorkers

The program calculates salary for several types of workers.
For each worker contains the models and classes necessary for payroll.
Each type of worker has its own type of calculator working according to the visitor pattern.

Pros:
1. Data models are encapsulated in interfaces, which allows you to use any implementation for the models themselves.
For example, it can be an Entity from a database or classes using serialization or something else.
2. The functionality for working with the entities of workers is implemented through the IWorkerStorage interface, which allows you to hide the logic of obtaining / saving data directly
3. The Facade pattern (IWorkerService) allows you to add any behavior before or after working with the data entities themselves, such as caching. Although in this production it was possible to do without it.
4. For calculators, the Template Method pattern was used. This allows using the same steps of calculation algorithms.
5. The visitor pattern was used for calculations. This allows:
- do not add unnecessary properties and methods to the IWorker interface and any other.
- to avoid transformations (casting) in the project code as much as possible.
- quickly create new classes based on existing ones without changing existing interfaces.
But transformations will be inevitable in specific implementations of calculators if they work with successors that extend IWorker.
Also, when working with subordinates, you will have to cast IWorker to ITopWorker.

This could have been avoided by making one data interface for Employee, Manager, Sales and the methods in Employee would do nothing.
This would make it easier to create a calculator base class that works according to the template method pattern.
But then in Employee it would be possible to refer to methods and members that do not carry functionality, and they would also take up memory space.


Minuses:
1. The program is designed for single-threaded work. This will result in errors when using IWorkerStorage multi-threaded.
2. Adding new classes is strictly tied to Enum WorkerType.


Offers:
1. I can try to improve the speed of calculating the salary due to multithreading.
As long as the data on employees is in RAM, the gain will not be very significant.
If the data is in the database, the gain will be significant, since you do not have to wait for a response from the database each time.
2. Now the worker and the calculator are strictly related to the workerType, but for more flexibility, the calculator type property could be used.
3. The creation of a new worker could be done in principle by the IWorkersService function.

I hid most of the controversial issues under interfaces so that they can be resolved when a specific task or technology already appears.
