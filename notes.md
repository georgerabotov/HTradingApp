# Develop a bonus management System - Calculate Bonus points, Apply different Algorithms and allow extending on them.
- It should be able to create thousands of credit records into our user’s accounts at the
end of each month.
- Sync trading data with trading platform
- Credit eligible accounts with bonus points

# Current approach
- Manually gather data
- Summarize using Excel
- Calculate points for each user then add credit (Manually)
- The idea is to Automate this.

# The code
- TDD Approach
- Write tests, add commit when failing, commit when passing, commit when refactored

# The notes
- User focused. Give very clear explanation what is the problem if there is an error.

# Storage
- Since I am mocking / storing the data somewhere, I would use a database, but since I am currently on my mac since I am travelling,
I would spend a lot of time having to start a docker container and so on. To save time, I have decided to use InMemoryCache and just update that.
I used the memory cache for both the mock and the storage of bonus points. This way whoever tests it , wouldn't have to worry about running DB

# My approach
- Let's break down the problem into multiple smaller problems:
    1. "You'll be working on interfaces around trading platform" - That is self-explanatory, I think it also explains what the task is.
    2. Calculate bonus points based on Deals.
    3. Check eligibility? Write some sort of a check that verifies if the user if eligible for bonus points
    4. Be able to apply different Algorithms and add new ones (Here, we are talking about clean and code that is future-proof and scalable)
    5. Be able to create thousands of credit records into the user's accounts at the end of each month.
        - Here we have multiple approaches, since this is happening at the end of each month.
        i. We can create a hosted service, that is going to check for deals and calculate bonus points (also add a feature flag to enable it.)
        ii. Since we are building an API, we can have an endpoint that triggers a report, that does the calculation and adds the points to the database
        iii. Calculate and add bonus points every time there is a deal using event-driven architecture. (That is usually the most complex way)
        This approach is the one I will skip as this is an interview test but I did want to point out that this is probably the best approach
        for continues update of data. Also not applicable as in the spec it says "End of each month"
        - For the sake of demonstration, I will use the method ii. which is the endpoint that I am creating.

- Firstly, make sure to handle use cases correctly and handle data correct, then return meaningful messages.
- Status code 500 usually means that we have no idea what happened. That is also something that's not very pleasing for the user
- To handle data correctly, I will use fluentValidation, which gives a lot of flexibiltiy
- For the architecture of the application, I will separate in 3 layers, Web, Domain and Persistence.
- Web will be the API, which will be calling the interfaces.
- In the domain layer, I will add the structure of the application Models, Interfaces ( usually for persistence layer we have separate models
    because it's not correct to use the Entity models everywhere but I will leave it for now for demo purposes)
- In the persistence layer, I will be doing all the work related to the calculation of the bonus points as well as getting the data from the trading api mock

# Application will automatically populate 5 users
- Their Ids will be from 1 to 5.

# First of all, I decided to use Mediatr and fluent validation as it allows me to create requests and write very clean code and add the validation for it. 
- That way, if something in the validation fails, we will get message that is valuable. The one thing that I would change, is the Must method on the validators
- Where I am validating whether the account exists in the data store, it will return 500 instead of bad request.

# In order to improve user's experience we need:
- Clients always come first! The customer is always right. 
1. Give meaningful messages in terms of what is happening when they make a request
2. Make sure it's fast, if it's not, make it clear that operation is underway.
3. The customer should know exactly what happens after a certain click or action they have taken, meaning that the app should be self-explanatory

# How would you run sync with prod.
- There is multiple ways to go about it. I mentioned the hosted service and event driven architecture.
- Since we are running our bonus points calculation at the end of the month, we only have to worry about failures, but if it's consistently doing that
- we can do the following:
i. Add messages queues, if we use AWS, we can use SQS, if Azure, we can use Azure bus service or azure event hub.
ii. Use Locks / Semaphores, that way nothing else can update entities.
iii. if we are using a hosted service, we can check for any differences on certain periods.
iv. Scale out, it's always good to have backup in case of disaster, either multiple regions/ availability zones and since we use message queues
they will just be redirected to the correct location
v. never interact with database during high-volume times, meaning that it's always best to run reports or do things like that, outside the
hours in which people are working / interacting with the database.

# How would you communicate errors and problems to the user? 
- First of all, transparency is important
- Communication with a client is always important to be straight forward. For example:
"Dear customer, we have experienced an outage in one of our systems, we are working to bring it back to a stable state as soon as possible,
Apologies for the inconvenience caused. Kind regards -- ..."
- If it's high-pressure and lots of money involved, it's always important to make sure that we are going to take responsibility and solve the issue,
regardless what it is
- Customers are essentially the people that bring money into the company, it's always important to provide them with the best possible experience.

# What tradeoffs did you make in your solution? How does this tradeoff impact the
user? Why did you make the decision you made?
- Firstly, MediatR is not the fastest framework, but allows us to write very clean code and very often to implement CQRS pattern if we want to
segregate writes from reads (commands from queries). One of the important points was to write code that is clean and code that we can add upon or
make changes, so if we want to add different functions, that's fine.
- Secondly, I think I added an extra layer of complexity, which is not always understandable, may be if a junior developer looks at the code it will
take some time to wrap his/her head around the code to understand what the solution is. However, the reason I've used a repository pattern and inversed it
is to be able to avoid VENDOR LOCK-IN. For example, if we are using SQL and we need to migrate to Graph db in 3 months, we can just do it by
switching the persistence layer. The only downside to it is that you cannot use the same models for the whole app, but in any case, customer should
never be able to see sensitive information like id and etc etc. This API is only for purpose of the task, hence, why the id is int instead of guid.

- Tradeoff for cache, again, making it more complex but that's what I could use at the moment as I am on my macbook due to travelling instead of
being on my windows pc, and made no sense to spin up a database instance. I decided to use cache, because I populate on start and then it just works, just
run the app and don't worry about it.

# What would you include in the next iteration of this feature?
1. Better calculations, may be I just did basic ones.
2. I would definitely change the id from int to UUID or ULID or GUID.
3. Add User interface
4. Make the endpoints more robust, less chance to get nulls and handle multiple errors
5. I would handle errors better, the ErrorHandlingMiddleware could end up returning errors that the customer doesn't need to see, which is security
implication.
6. Change the Must() validation on the Validators to return Bad request instead of Internal Server error with the error message.

# If you wanted to implement something, but didn't have time for it, tell us about it, and
why it was important.
- From the points above, I wanted todo 2, 4, 5, 6 first. Mostly related to errors and user expierience ,that would make the application a whole lot better
- As for 2, it would just allow more flexibility and wouldn't have to worry about increments, because if we use event driven architecture to
create bonus points and we get 500 requests to create BonusPoints, we might end up having only 2 in the database. That needs a bit more work.
Adding queues or working with semaphores/locks.
- Some methods are asynchronous but they don't do any async operations, that was due to change in the way calculations worked. That can go as
technical debt and we done as the next thing. Either make the calculations asynchronous or make the methods, handlers synchronours as they expect to return
a task , or await a task.

# Any other comments, questions, or thoughts that came up.
- I made up the calculations for the bonus points, I couldn't find it anywhere , hopefully that's fine.
- SCOPE: I've used mainly Scoped when building the app services, since I want it to create instances of resource for every request but re-use it
instead of creating multiple instances which is slower and not good for memory. However for errors it's important to use transient, that's why
I registered the error middleware transient. As for the cache, I've done it as singleton as this is essentially our datastore and we want to
make changes throughout the lifetime of the application.
- I would also change the validator to use the accountservice as well, but I wrote the code to use the cache directly for the sake of
unit test validation purpose and the fact that I am using cache instead of the db, otherwise I would mock it all. You can't mock what
IMemoryCache.Get returns unfortunately.