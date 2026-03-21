// Copyright (c) 2017 Pierre Milet. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
using pmilet.DomainEvents;
using Xunit;

namespace StonePaperScissorsApp.Tests
{
    /// <summary>
    /// Integration tests for the Stone-Paper-Scissors game using the domain events library.
    /// These replace the previous SpecFlow BDD scenarios with plain xUnit tests.
    /// </summary>
    public class StonePaperScissorsTests
    {
        private readonly IDomainEventDispatcher _dispatcher;
        private readonly Match _match;
        private readonly Player1 _player1;
        private readonly Player2 _player2;
        private readonly Outcome _outcome;

        public StonePaperScissorsTests()
        {
            _dispatcher = new DomainEventDispatcher();
            _match = new Match(_dispatcher);
            _player1 = new Player1(_dispatcher);
            _player2 = new Player2(_dispatcher);
            _outcome = new Outcome(_dispatcher);
        }

        // ---------------------------------------------------------------------------
        // Winning scenarios
        // ---------------------------------------------------------------------------

        [Fact]
        public void Stone_Beats_Scissors_Player1Wins()
        {
            _player1.Play(PlayType.Stone);
            _player1.Confirm();
            _player2.Play(PlayType.Scissors);
            _player2.Confirm();

            _match.End();

            Assert.Equal(PlayerType.Player1, _outcome.LastWinner());
        }

        [Fact]
        public void Scissors_Beats_Paper_Player1Wins()
        {
            _player1.Play(PlayType.Scissors);
            _player1.Confirm();
            _player2.Play(PlayType.Paper);
            _player2.Confirm();

            _match.End();

            Assert.Equal(PlayerType.Player1, _outcome.LastWinner());
        }

        [Fact]
        public void Paper_Beats_Stone_Player1Wins()
        {
            _player1.Play(PlayType.Paper);
            _player1.Confirm();
            _player2.Play(PlayType.Stone);
            _player2.Confirm();

            _match.End();

            Assert.Equal(PlayerType.Player1, _outcome.LastWinner());
        }

        [Fact]
        public void Stone_Beats_Scissors_Player2Wins()
        {
            _player1.Play(PlayType.Scissors);
            _player1.Confirm();
            _player2.Play(PlayType.Stone);
            _player2.Confirm();

            _match.End();

            Assert.Equal(PlayerType.Player2, _outcome.LastWinner());
        }

        [Fact]
        public void Scissors_Beats_Paper_Player2Wins()
        {
            _player1.Play(PlayType.Paper);
            _player1.Confirm();
            _player2.Play(PlayType.Scissors);
            _player2.Confirm();

            _match.End();

            Assert.Equal(PlayerType.Player2, _outcome.LastWinner());
        }

        [Fact]
        public void Paper_Beats_Stone_Player2Wins()
        {
            _player1.Play(PlayType.Stone);
            _player1.Confirm();
            _player2.Play(PlayType.Paper);
            _player2.Confirm();

            _match.End();

            Assert.Equal(PlayerType.Player2, _outcome.LastWinner());
        }

        // ---------------------------------------------------------------------------
        // Multi-round scenario
        // ---------------------------------------------------------------------------

        [Fact]
        public void MultiRound_Player1WinsMoreRounds_Player1IsWinner()
        {
            // Round 1: Player1 wins (Stone vs Scissors)
            _player1.Play(PlayType.Stone);
            _player1.Confirm();
            _player2.Play(PlayType.Scissors);
            _player2.Confirm();

            // Round 2: Player1 wins (Paper vs Stone)
            _player1.Play(PlayType.Paper);
            _player1.Confirm();
            _player2.Play(PlayType.Stone);
            _player2.Confirm();

            _match.End();

            Assert.Equal(PlayerType.Player1, _outcome.LastWinner());
        }

        [Fact]
        public void MultiRound_Player2WinsMoreRounds_Player2IsWinner()
        {
            // Round 1: Player2 wins (Stone vs Scissors)
            _player1.Play(PlayType.Scissors);
            _player1.Confirm();
            _player2.Play(PlayType.Stone);
            _player2.Confirm();

            // Round 2: Player2 wins (Paper vs Stone)
            _player1.Play(PlayType.Stone);
            _player1.Confirm();
            _player2.Play(PlayType.Paper);
            _player2.Confirm();

            _match.End();

            Assert.Equal(PlayerType.Player2, _outcome.LastWinner());
        }

        // ---------------------------------------------------------------------------
        // Score tracking
        // ---------------------------------------------------------------------------

        [Fact]
        public void After_Player1Wins_Player1ScoreIncremented()
        {
            _player1.Play(PlayType.Stone);
            _player1.Confirm();
            _player2.Play(PlayType.Scissors);
            _player2.Confirm();

            Assert.Equal(1, _match.player1Score);
            Assert.Equal(0, _match.player2Score);
        }

        [Fact]
        public void After_Player2Wins_Player2ScoreIncremented()
        {
            _player1.Play(PlayType.Scissors);
            _player1.Confirm();
            _player2.Play(PlayType.Stone);
            _player2.Confirm();

            Assert.Equal(0, _match.player1Score);
            Assert.Equal(1, _match.player2Score);
        }

        // ---------------------------------------------------------------------------
        // Add (delayed commit) semantics
        // ---------------------------------------------------------------------------

        [Fact]
        public void Play_BeforeConfirm_DoesNotUpdateScore()
        {
            _player1.Play(PlayType.Stone);
            _player2.Play(PlayType.Scissors);

            // No Confirm() called - scores should still be 0
            Assert.Equal(0, _match.player1Score);
            Assert.Equal(0, _match.player2Score);
        }
    }
}
