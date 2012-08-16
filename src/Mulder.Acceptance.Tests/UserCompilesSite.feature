Feature: user compiles site
	In order to serve my site with my favorite web server
	As a user
	I want to compile my site into a static web site

Scenario: compile default site
	Given I have just created a new mulder powered site
	And I have changed directories into the new mulder powered site
	When I run the compile command
	Then I should see "Loading site data..." message
	And I should see "Compiling site..." message
	And I should see "create [time] public/index.html" message
	And I should see "create [time] public/style.css" message
	And I should see "Site compiled in [time]s."
	And I should see configured output directory created
	And the configured output directory should be named "public"
	And I should see created files in configured output directory
	And the "public/index.html" file should contain the default home content
	And the "public/style.css" file should contain the default styles
