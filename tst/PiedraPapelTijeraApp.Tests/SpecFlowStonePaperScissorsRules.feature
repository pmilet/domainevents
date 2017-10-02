Feature: SpecFlowStonePaperScissorsRules
	In order to avoid silly mistakes
	As a stone paper scissors game player
	I want to be told who is the winner

@mytag
Scenario: Stone Wins Scissors
	Given player1 plays stone
	And   player2 plays scissors
	When  match ends
	Then  the winner is player1
