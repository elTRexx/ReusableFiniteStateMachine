#region CeCill-C license
#region English version
//Copyright Aurélien Pascal Maignan, (25 October 2026) 

//[aurelien.maignan@protonmail.com]

//This software is a computer program whose purpose is to provide a reusable state machine
//and provided as a library.

//This software is governed by the CeCILL-C license under French law and
//abiding by the rules of distribution of free software.  You can  use,
//modify and/ or redistribute the software under the terms of the CeCILL-C
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
//knowledge of the CeCILL-C license and that you accept its terms.
#endregion

#region French version
//Copyright Aurélien Pascal Maignan, (25 Octobre 2026) 

//aurelien.maignan@protonmail.com

//Ce logiciel est un programme informatique implémentant une machine à états réutilisable
//et fourni en tant que bibliothèque.

//Ce logiciel est régi par la licence CeCILL-C soumise au droit français et
//respectant les principes de diffusion des logiciels libres.Vous pouvez
//utiliser, modifier et/ou redistribuer ce programme sous les conditions
//de la licence CeCILL-C telle que diffusée par le CEA, le CNRS et l'INRIA 
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
//pris connaissance de la licence CeCILL-C, et que vous en avez accepté les
//termes. 
#endregion
#endregion

using ReusableFiniteStateMachine.Sources.StateMachine.Interfaces;
using ReusableFiniteStateMachine.Sources.States.Interfaces;

namespace ReusableFiniteStateMachine.Sources.States;

/// <summary>
/// The <see cref="BaseState"/> class that represent the starting state of any <see cref="IStateMachine"/> state machine.
/// </summary>
public class StartState : BaseState
{
  private IState? _firstState;

  /// <summary>
  /// <inheritdoc cref="BaseState.GetNextState(object?)"/>
  /// The next state to transition inot from this <see cref="StartState"/>. 
  /// This should be <see cref="_firstState"/> if initialized or falling back 
  /// to its given <paramref name="argument"/> as an <see cref="IState"/> if not.
  /// </summary>
  /// <param name="argument">The potential falling back next <see cref="IState"/> to transition inot from this <see cref="IState"/>.</param>
  /// <returns>The next <see cref="IState"/> to transition into from this <see cref="StartState"/> (should ussually be <see cref="_firstState"/>).</returns>
  public override IState? GetNextState(object? argument = null)
    => _firstState ?? argument as IState;

  /// <summary>
  /// <inheritdoc cref="BaseState.Init(object?)"/>
  /// Initialize this <see cref="StartState"/> with given <paramref name="argument"/> as its <see cref="_firstState"/>.
  /// </summary>
  /// <param name="argument">The <see cref="IState"/> object that this <see cref="StartState"/> should transition into when its <see cref="IStateMachine"/> state machine owner start its running.</param>
  internal protected override void Init(object? argument = null)
    => _firstState = argument as IState; 

  /// <summary>
  /// <inheritdoc cref="BaseState.OnEnter(IState?)"/>
  /// Raise <see cref="IStateMachine.MachineStarted"/> event when entering this <see cref="StartState"/>.
  /// </summary>
  /// <param name="previousState">Not needed here.</param>
  public override void OnEnter(IState? previousState)
    => StateMachineOwner?.RaiseStateMachineStarted();

  /// <summary>
  /// We directly transition away from this <see cref="StartState"/> without any logic needed here.
  /// </summary>
  /// <param name="nextState">Not needed here.</param>
  public override void OnExit(IState? nextState)
  { }

  /// <summary>
  /// <see cref="StartState"/> is the start step of any <see cref="IStateMachine"/> state machine,
  /// and there is no logic updating anything in this state as we directly transition into its next <see cref="IState"/>
  /// when its <see cref="IStateMachine"/> state machine owner start running.
  /// </summary>
  /// <param name="argumentObject">Not needed here.</param>
  public override void Update(object? argumentObject = null)
  { }
}
