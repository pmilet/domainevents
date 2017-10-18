// Copyright (c) 2017 Pierre Milet. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
using Microsoft.VisualStudio.TestTools.UnitTesting;
using pmilet.DomainEvents;
using System;
using TechTalk.SpecFlow;

namespace StonePaperScissorsApp.Tests
{
    [Binding]
    public class ReglasStonePaperScissorsSteps
    {
        private readonly IDomainEventBus bus;
        private readonly Match match;
        private readonly Player1 player1;
        private readonly Player2 player2;
        private readonly Outcome outcome;
        public ReglasStonePaperScissorsSteps()
        {
            bus = new DomainEventBus();
            match = new Match(bus);
            player1 = new Player1(bus);
            player2 = new Player2(bus);
            outcome = new Outcome(bus);
        }

        [Given(@"player1 plays stone")]
        public void GivenPlayer1PlaysStone()
        {
            player1.Play(PlayType.Stone);
            player1.Confirm();
        }
        
        [Given(@"player2 plays scissors")]
        public void GivenPlayer2PlaysScissors()
        {
            player2.Play(PlayType.Scissors);
            player2.Confirm();
        }
        
        [When(@"match ends")]
        public void WhenMatchEnds()
        {
            match.End();
        }
        
        [Then(@"the winner is player1")]
        public void ThenTheWinnerIsPlayer1()
        {
            PlayerType expectedWinner = PlayerType.Player1;
            var actual = outcome.LastWinner();
            Assert.AreEqual<PlayerType>(expectedWinner, actual);
                
        }
    }

    
}
