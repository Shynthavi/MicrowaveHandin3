using System;
using Microwave.Classes.Boundary;
using Microwave.Classes.Controllers;
using Microwave.Classes.Interfaces;
using NSubstitute;
using NUnit.Framework;

namespace Microwave.Test.Integrationtest
{
    [TestFixture]
    public class IntegrationStep2
    {
        private IButton _powerButton;
        private IButton _timeButton;
        private IButton _startCancelButton;
        private IDoor _door;
        private ICookController _cookController;
        private IDisplay _display;
        private ILight _light;
        private IUserInterface _userInterface;
        private IOutput _output;

        [SetUp]
        public void Setup()
        {
            _powerButton = new Button();
            _timeButton = new Button();
            _startCancelButton = new Button();
            _door = new Door();
            _cookController = Substitute.For<ICookController>();
            _display = new Display(_output);
            _output = Substitute.For<IOutput>();
            _light = new Light(_output);

            _userInterface = new UserInterface(_powerButton, _timeButton, _startCancelButton, _door, _display, _light,
                _cookController);
        }


        #region Light

        [Test]
        public void Door_Opened_LightOn_Output()
        {
            //Act
            _door.Open();

            //Assert
            _output.Received(1).OutputLine("Light is turned on");
        }

        [Test]
        public void Door_Closed_LightOff_Output()
        {
            //Act
            _door.Open();
            _door.Close();

            //Assert
            _output.Received(1).OutputLine("Light is turned off");
        }
        #endregion

        #region Display

        //ShowPower(), ShowTime(), Clear()


        [Test]
        public void PowerButton_PressOnce_ShowPower()
        { 
            //Act
            _powerButton.Press();

            //Assert
            _output.Received(1).OutputLine("Display shows: 50 W");
            }

        [Test]
        public void TimerButton_PressOnce_ShowTime()
        {
            //Act
            _powerButton.Press();
            _timeButton.Press();

            //Assert
            _output.Received(1).OutputLine("Display shows: 01:00");
        }

        [Test]
        public void Display_Clear()
        { 
            //Act
            _powerButton.Press();
            _timeButton.Press();
            _startCancelButton.Press();
            _door.Open();

            //Assert
            _output.Received(1).OutputLine("Display cleared");
        }
        #endregion

    }
}