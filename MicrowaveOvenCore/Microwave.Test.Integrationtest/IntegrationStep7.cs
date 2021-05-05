using System;
using System.Collections.Generic;
using System.Text;
using Microwave.Classes.Boundary;
using Microwave.Classes.Controllers;
using Microwave.Classes.Interfaces;

using NSubstitute;
using NUnit.Framework;

namespace Microwave.Test.Integrationtest
{
    public class IntegrationStep7
    {
        private IButton _powerButton;
        private IButton _timeButton;
        private IButton _startCancelButton;
        private CookController _cookController;
        private IDisplay _display;
        private ITimer _timer;
        private IUserInterface _userInterface;
        private IOutput _output;
        private IDoor _door;
        private ILight _light;
        private IPowerTube _powerTube;


        [SetUp]
        public void Setup()
        {

            _powerButton = new Button();
            _timeButton = new Button();
            _startCancelButton = new Button();
            _output = Substitute.For<IOutput>();
            _door = new Door();
            _light = new Light(_output);
            _display = new Display(_output);
            _timer = Substitute.For<ITimer>();
            _powerTube = new PowerTube(_output);
            _cookController = new CookController(_timer,_display,_powerTube);
            _userInterface = new UserInterface(_powerButton, _timeButton, _startCancelButton, _door, _display, _light,
                _cookController);
            _cookController.UI = _userInterface;

        }

        //ShowTime()

        [Test]
        public void OnTimerTick_CookController()
        {
            //Act
            _powerButton.Press();
            _timeButton.Press();
            _startCancelButton.Press();

            _timer.TimerTick += Raise.EventWith(this, EventArgs.Empty);
            //Assert
            //_display.Received(1).ShowTime(1, 0);
            _output.Received(1).OutputLine("Display shows: 01:00");

        }

        [Test]
        public void OnTimerTick_CookController_2()
        {
            //Act
            _powerButton.Press();
            _timeButton.Press();
            _timeButton.Press();
            _startCancelButton.Press();

            _timer.TimerTick += Raise.Event();
            _timer.TimerTick += Raise.Event();

            //Assert
            //_display.Received(1).ShowTime(1, 0);
            _output.Received(1).OutputLine("Display shows: 02:00");

        }


    }
}
