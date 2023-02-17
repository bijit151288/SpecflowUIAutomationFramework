Feature: Login Page
Different test scenarios for Login Page

Background:
	Given Launch the Zeus application
	But User is not logged in

@TC_Zeus_Login_0001 @PositiveTests
Scenario: Verify User access when login information is valid
	Given I provide all required fields with valid data
    #Using valid Username and password
	When I click on Login
	Then User should be redirected to Zeus main page after being prompted with message "Please wait. Zeus is setting up the configurations for you!!"

@TC_Zeus_Login_0002 @NegativeTests
Scenario: Verify User access when Username is valid and password is incorrect
	Given I provide all required fields with incorrect password
    #Using Valid Username and invalid password
	When I click on Login
	Then User should be prompted with message "Invalid Credentials. You have 4 more attempt(s) left. To generate new password, use Forgot Password option."
    #If attempted more times message should be the same while the number of attempts should be attempts left - 1
	Given I provide all required fields with incorrect password
	When I click on Login
	Then User should be prompted with message "Invalid Credentials. You have 3 more attempt(s) left. To generate new password, use Forgot Password option."

@TC_Zeus_Login_FP_0003 @PositiveTests
Scenario: Verify Forgot Password while Username is valid
	Given I open Forgot Password Page
	And I provide Valid Username
		| ValidUsername |
		| aaly          |
	When I click Reset Password
	Then User should be redirected to the Login Page and receive "Email sent to registered Email"