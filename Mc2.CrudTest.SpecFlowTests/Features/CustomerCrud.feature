Feature: User can Create, Update, Read And Delete customers
 
"""
Customer {
	Firstname
	Lastname
	DateOfBirth
	PhoneNumber
	Email
	BankAccountNumber
}
 
Domain
Application 
Infrastructure
Presentation
 
"""
 
Background:
  Given platform support following error codes
    | Code | Description                                  |
    | 101  | Invalid Email                                |
    | 102  | Invalid PhoneNumber                          |
    | 103  | Invalid Bank Account Number                  |
    | 201  | Duplicated FirstName, Lastname in data store |
    | 202  | Duplicated Email                             |
 
 
Scenario: User send query to Create, Update, Read and Delete customers
    Given platform has "0" records of customers
 
    When user send following request to create a new customer
      | Firstname | Lastname | DateOfBirth | PhoneNumber   | Email              | BankAccountNumber |
      | John      | Doe      | 19-JUN-1999 | +989050617876 | john.doe@gmail.com | NL91RABO0317001297|
    Then administrator can query and get "1" record of user with below information
      | Firstname | Lastname | DateOfBirth | PhoneNumber   | Email              | BankAccountNumber |
      | John      | Doe      | 19-JUN-1999 | +989050617876 | john.doe@gmail.com | NL91RABO0317001297|
 
    When user send following request to create a new customer
      | Firstname | Lastname   | DateOfBirth | PhoneNumber   | Email              | BankAccountNumber |
      | John      | Smith      | 19-JUN-1989 | +989050617876 | john.doe@gmail.com | NL91RABO0317001297|
    Then user will receive following error codes
      | Code |
      | 202  |
 
    When user send following request to create a new customer
      | Firstname | Lastname   | DateOfBirth | PhoneNumber   | Email              | BankAccountNumber |
      | Jeff      | Smith      | 19-JUN-1989 | +9050617876   | john.doe@gmail.com | NL91RABO03170017  |
    Then user will receive following error codes
      | Code |
      | 103  |
      | 102  |
      | 202  |
 
    When user send following request to update a customer with email "john.doe@gmail.com" with following information
      | Firstname | Lastname   | DateOfBirth | PhoneNumber   | Email                | BankAccountNumber |
      | John      | Smith      | 19-MAY-1999 | +989050617877 | john.smith@gmail.com | NL91RABO03170018  |
    Then administrator can query and get "1" record of user with below information
      | Firstname | Lastname   | DateOfBirth | PhoneNumber   | Email                | BankAccountNumber |
      | John      | Smith      | 19-MAY-1999 | +989050617877 | john.smith@gmail.com | NL91RABO03170018  |
    And administrator can query and get "0" record of user with below information
      | Firstname | Lastname | DateOfBirth | PhoneNumber   | Email              | BankAccountNumber |
      | John      | Doe      | 19-JUN-1999 | +989050617876 | john.doe@gmail.com | NL91RABO0317001297|
 
    When user send request to delete a customer with email "john.smith@gmail.com"
    Then administrator query to get all customers and get "0" records of customers
 