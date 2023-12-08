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

# My approach
- Let's break down the problem into multiple smaller problems:
    1. "You'll be working on interfaces around trading platform" - That is self-explanatory, I think it also explains what the task is.
    2. Calculate bonus points based on Deals.
    3. Be able to apply different Algorithms and add new ones (Here, we are talking about clean and code that is future-proof and scalable)
    4. Be able to create thousands of credit records into the user's accounts at the end of each month.
        - Here we have multiple approaches, since this is happening at the end of each month.
        i. We can create a hosted service, that is going to check for deals and calculate bonus points (also add a feature flag to enable it.)
        ii. Since we are building an API, we can have an endpoint that triggers a report, that does the calculation and adds the points to the database
        iii. Calculate and add bonus points every time there is a deal using event-driven architecture. (That is usually the most complex way)
        This approach is the one I will skip as this is an interview test but I did want to point out that this is probably the best approach
        for continues update of data. Also not applicable as in the spec it says "End of each month"

- Firstly, make sure to handle use cases correctly and handle data correct, then return meaningful messages.
- Status code 500 usually means that we have no idea what happened. That is also something that's not very pleasing for the user
- To handle data correctly, I will use fluentValidation, which gives a lot of flexibiltiy
- To create Moq data for the app, I will use Bogus faker which will create different inputs every time.
- For the architecture of the application, I will separate in 3 layers, Web, Domain and Persistence.
- Web will be the API, which will be calling the interfaces.
- In the domain layer, I will add the structure of the application Models, Interfaces ( usually for persistence layer we have separate models
    because it's not correct to use the Entity models everywhere but I will leave it for now for demo purposes)
- In the persistence layer, I will be doing all the work related to the calculation of the bonus points.


