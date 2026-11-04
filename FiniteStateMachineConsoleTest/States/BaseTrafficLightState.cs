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

using ReusableFiniteStateMachine.Sources.States.Interfaces;
using ReusableFiniteStateMachine.Sources.States;
using ReusableFiniteStateMachine.Sources.StateMachine.Interfaces;
using ReusableFiniteStateMachine.Tests.Helpers;

namespace FiniteStateMachineConsoleTest.States
{
  internal abstract class BaseTrafficLightState : BaseState
  {
    protected int _tick = 0;
    protected int _delay = 2;

    /// <summary>
    /// Simple internal method to increase the count of how many time 
    /// the <see cref="Common.StoppableTimer"/> have ticked when this <see cref="IState"/>
    /// is the current active state of its <see cref="IStateMachine"/> state machine owner.
    /// </summary>
    internal void Tick() => _tick++;

    /// <summary>
    /// <inheritdoc cref="BaseState.Init(object?)"/>
    /// Initialize a new <see cref="Timer"/> for its <see cref="_timer"/> field.
    /// </summary>
    /// <param name="argument">The <see cref="_timer"/> delay in second(s) as an <see langword="int"/> (default is 2 seconds)..</param>
    protected override void Init(object? argument = null)
      => _delay = argument is int delay ? delay : 2;

    /// <summary>
    /// <inheritdoc cref="BaseState.OnEnter(IState?)"/>
    /// Assure to reset its <see cref="_tick"/> field.
    /// </summary>
    /// <param name="previousState">Not used here.</param>
    public override void OnEnter(IState? previousState)
    {
      Console.WriteLine($"\n--- [{DateTime.Now:mm:ss:ff}] Entering => {GetType().Name} ---\n");

      ///Reset inner timer's count before starting the (shared) Timer.
      _tick = 0;

      Common.StoppableTimer.Start(TimeSpan.FromSeconds(_delay));
    }

    /// <summary>
    /// <inheritdoc cref="BaseState.OnExit(IState?)"/>
    /// Exiting this <see cref="BaseTrafficLightState"/> stop <see cref="Common.StoppableTimer"/>.
    /// </summary>
    /// <param name="nextState">Not used here.</param>
    public override void OnExit(IState? nextState)
    {
      Common.StoppableTimer.Stop();
      
      Console.WriteLine($"\n--- [{DateTime.Now:mm:ss:ff}] Exiting <= {GetType().Name} ---\n");      
    }

    /// <summary>
    /// <inheritdoc cref="BaseState.Update(object?)"/>
    /// No update logic performed here.
    /// </summary>
    /// <param name="argument">Not used here.</param>
    public override void Update(object? argument = null)
    { }
  }
}
