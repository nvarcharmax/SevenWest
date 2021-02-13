
# SevenWest

## Design Principles

- Simplicity and readability

My view of modern software development philosophies revolves around simplicity, readability and ability to make and ship changes quickly. This is aided by modern Devops tooling and practices as well as the fact that most tactical decisions are easily reversible.

- Testability

Key parts of the solution is made testable wherever suitable, especially with the queries and abstractions of remote Http calls

## Project structure

There are 2 main projects:

- SevenWest.Console
- SevenWest.Core

## Assumptions

- The data provided in example_data.json contradicts with "File formatted with JSON on every row". 
I'm assuming here that the json file contains an array of json objects

- There are genders other than M or F. 
Without making any further incorrect assumptions, I will accept the new genders provided in the file and process them accordingly. 
If I were to "validate" those genders, then I would either create mapping to the "traditional" M or F or exclude the data for consumption

- In the sample data, there are missing quotes around the property "gender", this will be handled by the Newtonsoft Json library

## Considerations

1. The data source may change.
For this requirement, the IPersonDataSource is abstracted and can be replaced by a different implementation.

2. The endpoint could go down.
Error handling is added around the Http operation. Furthermore, if successful, the response is cached for 1 minute, and thus the endpoint will not be hit again until expiry

3. The endpoint has known to be slow in the past.
A cache has been added for the Http response so that the impact is minimal for subsequent "Api calls"

4. The way source is fetched may change.
Similar to requirement 1.

5. The number of records may change (performance).
Similar to 3. 
As there is no backing store for this, all the operations are performed in memory. 
For large datasets, I could pre-optimise the search operations such as looking up by Id or Age. However, this could be construed as premature optimisations. 
If for example, if we had a SQL backend store, it would be better to perform the filter and grouping operations within the SQL engine itself

6. The functionality may not always be consumed in a console app.
The project is structured to contain a Core and a Console project. 
The output of the queries are returned in IOutputService such that a future Web project may accept the results and present them accordingly

## Notes

- I previously attempted a version of this test 2 years ago. However a remote Api call was not involved then

- The Unit Tests have been extended to support fake Http responses back to support multiple Http operations
