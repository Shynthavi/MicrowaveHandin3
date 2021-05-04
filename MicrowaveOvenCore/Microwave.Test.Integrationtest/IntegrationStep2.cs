using System;
using System.Collections.Generic;
using System.Text;
using Microwave.Classes.Controllers;
using Microwave.Classes.Interfaces;
using NSubstitute;
using NUnit.Framework;

namespace Microwave.Test.Integrationtest
{
   public class IntegrationStep2
    {
        private IButton _powerButton;
        private IButton _timeButton;
        private IButton _cancelStartButton;
        private IDoor _door;
        private ICookController _cookController;
        private IDisplay _display;
        private ILight _light;
        private UserInterface _userInterface;


        [SetUp]
        public void Setup()
        {
            _powerButton = Substitute.For<IButton>();
            _timeButton = Substitute.For<IButton>();
            _cancelStartButton = Substitute.For<IButton>();
            _door = Substitute.For<IDoor>();
            _cookController = Substitute.For<ICookController>();
            _display = Substitute.For<IDisplay>();
            _light = Substitute.For<ILight>();
            //
            _userInterface = new UserInterface(_powerButton, _timeButton, _cancelStartButton, _door, _display, _light, _cookController);
        }
    }
}
