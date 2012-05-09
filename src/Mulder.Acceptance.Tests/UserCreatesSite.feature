Feature: user creates site
	As a user
	I want a minimal site created for me
	So that I don't have to start from scratch

Scenario: create site without a path
	When I run the create site command without a path
	Then I should see usage message
	And I should see mulder terminate with an error exit code

Scenario: create site with path that already exists
	Given I have a path that already exists
	When I run the create site command with a path that already exists
	Then I should see "A site at '[path]' already exists." message
	And I should see mulder terminate with an error exit code due to path existing

Scenario: create site with valid path
	Given I have a valid path
	When I run the create site command with a valid path
	Then I should see "create config.yaml" message
	And I should see "create Rules" message
	And I should see "create layouts/default.html" message
	And I should see "create content/index.html" message
	And I should see "create content/stylesheet.css" message
	And I should see "Created a blank mulder site at '[path]'. Enjoy!" message
	And I should have a directory created for my site
	And I should have my site directory populated with the default bare-bones site
	And the "config.yaml" file should contain the default config
	And the "Rules" file should contain the default rules
	And the "layouts/default.html" file should contain the default layout
	And the "content/index.html" file should contain the default content
	And the "content/stylesheet.css" file should contain the default styles
	And mulder should terminate with an success exit code
