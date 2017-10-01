Feature: ReglasPiedraPapelTijera
	Con el fin de determinar el ganador
	Como arbitro del Juego
	Quiero saber el ganador para cada uno de las jugadas del juego piedra papel tijera

@mytag
Scenario: Dado que Piedra Gana a Tijera 
	Given Jugador1 juega Piedra
	And y Jugador2 juega Tijera
	When Finalizo la partida
	Then El ganador es Jugador1

	