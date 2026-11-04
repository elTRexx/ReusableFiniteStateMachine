#region CeCill-B license
#region English version
//Copyright Aurélien Pascal Maignan, (25 October 2026) 

//[aurelien.maignan@protonmail.com]

//This software is a computer program whose purpose is
//to test the reusable state machine software library named "ReusableFiniteStateMachine"
//using TUnit testing framework by Tom Longhurst (https://github.com/thomhurst/TUnit)

//This software is governed by the CeCILL-B license under French law and
//abiding by the rules of distribution of free software.  You can  use,
//modify and/ or redistribute the software under the terms of the CeCILL-B
//license as circulated by CEA, CNRS and INRIA at the following URL
//"http://www.cecill.info". 

//As a counterpart to the access to the source code and  rights to copy,
//modify and redistribute granted by the license, users are provided only
//with a limited warranty  and the software's author,  the holder of the
//economic rights, and the successive licensors  have only  limited
//liability. 

//In this respect, the user's attention is drawn to the risks associated
//with loading,  using,  modifying and/or developing or reproducing the
//software by the user in light of its specific status of free software,
//that may mean  that it is complicated to manipulate, and  that  also
//therefore means  that it is reserved for developers  and  experienced
//professionals having in-depth computer knowledge. Users are therefore
//encouraged to load and test the software's suitability as regards their
//requirements in conditions enabling the security of their systems and/or 
//data to be ensured and, more generally, to use and operate it in the 
//same conditions as regards security. 

//The fact that you are presently reading this means that you have had
//knowledge of the CeCILL-B license and that you accept its terms.
#endregion

#region French version
//Copyright Aurélien Pascal Maignan, (25 Octobre 2026) 

//aurelien.maignan@protonmail.com

//Ce logiciel est un programme informatique servant à tester
//la bibliothèque de machine à états réutilisable dénomé "ReusableFiniteStateMachine"
//en utilisant le cadriciel TUnit développé par Tom Longhurst (https://github.com/thomhurst/TUnit)

//Ce logiciel est régi par la licence CeCILL-B soumise au droit français et
//respectant les principes de diffusion des logiciels libres.Vous pouvez
//utiliser, modifier et/ou redistribuer ce programme sous les conditions
//de la licence CeCILL-B telle que diffusée par le CEA, le CNRS et l'INRIA 
//sur le site "http://www.cecill.info".

//En contrepartie de l'accessibilité au code source et des droits de copie,
//de modification et de redistribution accordés par cette licence, il n'est
//offert aux utilisateurs qu'une garantie limitée.  Pour les mêmes raisons,
//seule une responsabilité restreinte pèse sur l'auteur du programme,  le
//titulaire des droits patrimoniaux et les concédants successifs.

//A cet égard  l'attention de l'utilisateur est attirée sur les risques
//associés au chargement, à l'utilisation,  à la modification et/ou au
//développement et à la reproduction du logiciel par l'utilisateur étant 
//donné sa spécificité de logiciel libre, qui peut le rendre complexe à
//manipuler et qui le réserve donc à des développeurs et des professionnels
//avertis possédant  des  connaissances  informatiques approfondies.Les
//utilisateurs sont donc invités à charger  et  tester  l'adéquation  du
//logiciel à leurs besoins dans des conditions permettant d'assurer la
//sécurité de leurs systèmes et ou de leurs données et, plus généralement,
//à l'utiliser et l'exploiter dans les mêmes conditions de sécurité.

//Le fait que vous puissiez accéder à cet en-tête signifie que vous avez
//pris connaissance de la licence CeCILL-B, et que vous en avez accepté les
//termes. 
#endregion 
#endregion

// See https://aka.ms/new-console-template for more information
using FiniteStateMachineConsoleTest.States;
using ReusableFiniteStateMachine.Sources.StateMachine.Interfaces;
using ReusableFiniteStateMachine.Sources.Services;
using ReusableFiniteStateMachine.Tests.Helpers;

/// <summary>
/// Simple console application to test a simple simulated traffic light state machine
/// using <see cref="ReusableFiniteStateMachine"/> library project.
/// </summary>

Builder.BuildHost(args);

Console.WriteLine($"\n--- TESTING Reusable Finite State Machine ---\n");
var stateMachine = Builder.Services.StateMachine_Factory.CreateStateMachine();

if (stateMachine == null)
{
  Console.WriteLine($"\n--- ERROR created {nameof(IStateMachine)} is NULL! ---\n");
  return;
}

Console.WriteLine($"\n--- Created {stateMachine.GetType().Name} ---\n");

Common.StopableTimer.TimerCallback = _ =>
{
  if (stateMachine.CurrentState is BaseTrafficLightState lightState)
    lightState.Tick();
};

bool stopUpdateLoop = false;

/// <summary>
/// Simple event handler method used for <see cref="IStateMachine.MachineEnded"/> event
/// that set the <see cref="stopUpdateLoop"/> flag to <see langword="true"/>
/// enforcing <see cref="stateMachine"/> state machine to stop.
/// </summary>
void MachineEnded(object? sender, EventArgs e)
{
  Console.WriteLine($"\n--- ENDING Reusable Finite State Machine ---\n");
  stopUpdateLoop = true;
}

stateMachine.MachineEnded += MachineEnded;

long loopAmount = 0;
long maximumLoopAmount = 100_000_000_000;

var firstState = stateMachine.GetOrAddState<GreenState>(2);
//var firstState = Builder.Services.State_Factory?.GetOrCreateState<GreenState>(stateMachine, 2);

if (firstState == default)
{
  Console.WriteLine($"\n--- ERROR created {nameof(GreenState)} is NULL! ---\n");
  return;
}

Console.WriteLine($"\n--- Created {firstState.GetType().Name} ---\n");

Console.WriteLine($"\n--- STARTING Reusable Finite State Machine ---\n");

///Initilize <see cref="stateMachine"/> with <see cref="firstState"/> and automatically start running it.
stateMachine.InitializeStateMachine(firstState, true);

Console.WriteLine($" > {DateTime.Now:mm:ss}");

///Asynchronously run a task that will stop the running <see cref="stateMachine"/> state machine
///after 50 seconds to avoid being stucked in an eventual infinite updating loop.
Task.Run(
  async () => { await Task.Delay(50_000); stateMachine.ForceStopStateMachine(); }
  );

///Keep looping until <see cref="stateMachine"/> state machine end
///or safety <see cref="maximumLoopAmount"/> is reached (to prevent infinite loop).
while (!stopUpdateLoop && loopAmount < maximumLoopAmount)
  loopAmount++;

Console.WriteLine($" < {DateTime.Now:mm:ss}");

stateMachine.Dispose();

Common.StopableTimer.Dispose();

Console.WriteLine($"\n--- Requested stopping machine : {stopUpdateLoop}\n--- Played {loopAmount} loops ---\n");