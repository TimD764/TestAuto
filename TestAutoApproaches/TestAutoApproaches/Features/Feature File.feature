Feature: EHU Website End-to-End User Journey
  As a prospective student
  I want to navigate the EHU website
  So that I can find information about the university, studies, and contacts

  Scenario: Verify Navigation to 'About EHU' Page
    Given I navigate to the EHU home page
    When I click on the "About" tab in the main navigation
    Then I should be redirected to the About page
    And the page title should contain "About"

  Scenario Outline: Verify Search Functionality
    Given I navigate to the EHU home page
    When I search for "<searchTerm>"
    Then the search results page should include the query in the URL
    And the search results should contain links to study programs

    Examples: 
      | searchTerm     |
      | study programs |
      | admissions     |

  Scenario: Verify Language Change Functionality
    Given I navigate to the EHU home page
    When I accept cookies if present
    And I change the language to Lithuanian
    Then I should be redirected to the Lithuanian version of the site

  Scenario: Verify Contact Information
    Given I navigate to the EHU contact page
    Then the contact information should be visible to the user