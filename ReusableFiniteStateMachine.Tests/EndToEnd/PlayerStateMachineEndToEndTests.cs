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

using ReusableFiniteStateMachine.Tests.Fixture.StateMachineFixtures;
using ReusableFiniteStateMachine.Tests.Helpers;
using ReusableFiniteStateMachine.Sources.StateMachine.Interfaces;
using ReusableFiniteStateMachine.Sources.States.Interfaces;

namespace ReusableFiniteStateMachine.Tests.EndToEnd;

/// <summary>
/// Test class that encapsulate all end to end tests (that could take time to execute).
/// </summary>
public class PlayerStateMachineEndToEndTests
{
  /// <summary>
  /// End to end test that assert that a given running <see cref="PlayerStateMachineFixture"/> 
  /// should mostly eventually reach <see cref="IStateMachine.EndState"/> within 30 seconds,
  /// when generating and processing random <see cref="PlayerInputs"/> on the fly.
  /// </summary>
  /// <param name="playerStateMachine">The <see cref="PlayerStateMachineFixture"/> state machine fixture to test upon</param>
  /// <param name="cancellationToken">The <see cref="CancellationToken"/> used by this test as it has a associated <see cref="TimeoutAttribute"/> attribute.</param>
  /// <returns>The awaited test assertion.</returns>
  [Test]
  [Repeat(PLAYER_STATE_MACHINE_REPEAT-1)]
  [Timeout(PLAYER_STATE_MACHINE_TIMEOUT_MS)]
  [ClassDataSource<PlayerStateMachineFixture>]
  [NotInParallel]
  public async Task PlayerStateMachine_RandomlyProcessInputs_Should_Finally_Reach_EndState_Within_30Seconds(PlayerStateMachineFixture playerStateMachine, CancellationToken cancellationToken)
  {
    ///Arrange
    RepeatCount++;

    playerStateMachine.TransitionFailed += (_, _) => playerStateMachine.ForceStopStateMachine();

    playerStateMachine.CurrentStateChanged += (_, args) =>
      TestsLog.AppendLineShared($"[{args.PreviousState?.GetType().Name}] => ({CurrentInput}) => [{args.NewState?.GetType().Name}] - {(IsAlive ? "ALIVE" : "DEAD")} {CurrentCredit}/{DEFAULT_MAX_CREDIT} | {(playerStateMachine.IsEnded ? "ENDED" : "RUNNING")}");

    TestsLog.AppendLineShared(SEPARATOR);
    TestsLog.AppendLineShared($"New Player State Machine Test : {RepeatCount}/{PLAYER_STATE_MACHINE_REPEAT}");
    TestsLog.AppendLineShared(SEPARATOR);
    TestsLog.AppendLineShared($"Starting state is [{playerStateMachine.CurrentState?.GetType().Name}]\n");

    playerStateMachine.AutorunStateMachine();

    playerStateMachine.StartRandomInputsLoop();

    ///Act
    await playerStateMachine.AwaitStateMachineEndingAsync(cancellationToken);

    ///Assert
    await Assert.That(playerStateMachine.IsEnded && (CurrentCredit <= 0 || WasExitAsked)).IsTrue();
  }

  /// <summary>
  /// Static asynchronous method running after the execution of all test of this class.
  /// Used to log <see cref="PlayerStateMachine_RandomlyProcessInputs_Should_Finally_Reach_EndState_Within_30Seconds(PlayerStateMachineFixture, CancellationToken)"/> 
  /// test with the flow of <see cref="IState"/> thoses tests followed along the inputs they recieved, and attaching it as an artifact.
  /// </summary>
  /// <returns>The <see cref="Task"/> to be awaited for this method to finish (writting a log file on drive).</returns>
  [After(Class)]
  public static async Task WriteLogFilesAsync()
  {
    await File.WriteAllTextAsync(TESTS_LOG_PATH, TestsLog.ToString());

    TestsLog.Clear();

    if (File.Exists(TESTS_LOG_PATH))
    {
      TestContext.Current!.Output.AttachArtifact(new Artifact
      {
        File = new FileInfo(TESTS_LOG_PATH),
        DisplayName = "Player State Machine Log",
        Description = "End to End testing of a Player State Machine with random inputs within 30 seconds"
      });
    }
  }
}
