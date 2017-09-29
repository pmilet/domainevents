Feature: ReglasPiedraPapelTijera
	Con el fin de determinar el ganador
	Como arbitro del Juego
	Quiero saber el ganador de cada uno de las posibilidades del juego

@mytag
Scenario: Jugador1 juega Piedra y Jugador2 juega Tijera
	Given Jugador1 juega Piedra
	And y Jugador2 juega Tijera
	When Finalizo la partida
	Then El ganador es Jugador1
