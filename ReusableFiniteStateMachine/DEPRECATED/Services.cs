//#region CeCill-C license
//#region English version
////Copyright Aurélien Pascal Maignan, (25 October 2026) 

////[aurelien.maignan@protonmail.com]

////This software is a computer program whose purpose is to provide a reusable state machine
////and provided as a library.

////This software is governed by the CeCILL-C license under French law and
////abiding by the rules of distribution of free software.  You can  use,
////modify and/ or redistribute the software under the terms of the CeCILL-C
////license as circulated by CEA, CNRS and INRIA at the following URL
////"http://www.cecill.info". 

////As a counterpart to the access to the source code and  rights to copy,
////modify and redistribute granted by the license, users are provided only
////with a limited warranty  and the software's author,  the holder of the
////economic rights, and the successive licensors  have only  limited
////liability. 

////In this respect, the user's attention is drawn to the risks associated
////with loading,  using,  modifying and/or developing or reproducing the
////software by the user in light of its specific status of free software,
////that may mean  that it is complicated to manipulate, and  that  also
////therefore means  that it is reserved for developers  and  experienced
////professionals having in-depth computer knowledge. Users are therefore
////encouraged to load and test the software's suitability as regards their
////requirements in conditions enabling the security of their systems and/or 
////data to be ensured and, more generally, to use and operate it in the 
////same conditions as regards security. 

////The fact that you are presently reading this means that you have had
////knowledge of the CeCILL-C license and that you accept its terms.
//#endregion

//#region French version
////Copyright Aurélien Pascal Maignan, (25 Octobre 2026) 

////aurelien.maignan@protonmail.com

////Ce logiciel est un programme informatique implémentant une machine à états réutilisable
////et fourni en tant que bibliothèque.

////Ce logiciel est régi par la licence CeCILL-C soumise au droit français et
////respectant les principes de diffusion des logiciels libres.Vous pouvez
////utiliser, modifier et/ou redistribuer ce programme sous les conditions
////de la licence CeCILL-C telle que diffusée par le CEA, le CNRS et l'INRIA 
////sur le site "http://www.cecill.info".

////En contrepartie de l'accessibilité au code source et des droits de copie,
////de modification et de redistribution accordés par cette licence, il n'est
////offert aux utilisateurs qu'une garantie limitée.  Pour les mêmes raisons,
////seule une responsabilité restreinte pèse sur l'auteur du programme,  le
////titulaire des droits patrimoniaux et les concédants successifs.

////A cet égard  l'attention de l'utilisateur est attirée sur les risques
////associés au chargement, à l'utilisation,  à la modification et/ou au
////développement et à la reproduction du logiciel par l'utilisateur étant 
////donné sa spécificité de logiciel libre, qui peut le rendre complexe à
////manipuler et qui le réserve donc à des développeurs et des professionnels
////avertis possédant  des  connaissances  informatiques approfondies.Les
////utilisateurs sont donc invités à charger  et  tester  l'adéquation  du
////logiciel à leurs besoins dans des conditions permettant d'assurer la
////sécurité de leurs systèmes et ou de leurs données et, plus généralement,
////à l'utiliser et l'exploiter dans les mêmes conditions de sécurité.

////Le fait que vous puissiez accéder à cet en-tête signifie que vous avez
////pris connaissance de la licence CeCILL-C, et que vous en avez accepté les
////termes. 
//#endregion
//#endregion

//using ReusableFiniteStateMachine.Sources.Cache.Interface;
//using ReusableFiniteStateMachine.Sources.Cache;
//using ReusableFiniteStateMachine.Sources.Factories;
//using ReusableFiniteStateMachine.Sources.Factories.Interfaces;
//using ReusableFiniteStateMachine.Sources.StateMachine.Interfaces;

//namespace ReusableFiniteStateMachine.Sources.Utilities;

///// <summary>
///// Static class easing access to <see cref="IStateFactory"/> and <see cref="IStateMachineFactory"/> factories.
///// </summary>
//public static class Services 
//{
//  private static IStateCache? _stateCache;

//  public static IStateCache StateCache
//  {
//    get
//    {
//      if (_stateCache == null)
//        GetStateCache<StateCache>(true);
//      return _stateCache!;
//    }
//  }

//  private static IStateFactory? _stateFactory;
//  public static IStateFactory StateFactory
//  {  
//    get 
//    {
//      if (_stateFactory == null)
//        GetStateFactory<StateFactory>(true);
//      return _stateFactory!; 
//    } 
//  }

//  private static IStateMachineFactory? _stateMachineFactory;
//  public static IStateMachineFactory StateMachineFactory
//  {
//    get
//    {
//      if (_stateMachineFactory == null)
//        GetStateMachineFactory<StateMachineFactory>(true);
//      return _stateMachineFactory!;
//    }
//  }

//  /// <summary>
//  /// Get the cached <see cref="IStateFactory"/> or caching a new <typeparamref name="T_StateFactory"/> first 
//  /// if <see cref="_stateFactory"/> cached value field is <see langword="null"/>.
//  /// </summary>
//  /// <typeparam name="T_StateFactory">The actual <see cref="IStateFactory"/> implementing concrete type 
//  /// to be cached and returned if not already cached in the first place.</typeparam>
//  ///  <param name="forceNewInstance"><see langword="bool"/> flag indicating whether or not 
//  ///  reseting to <see langword="null"/> the cached <see cref="_stateFactory"/> field first (default is <see langword="false"/>).</param>
//  /// <returns>The already or to be cached <see cref="_stateFactory"/> <see cref="IStateFactory"/> instance.</returns>
//  public static IStateFactory GetStateFactory<T_StateFactory>(bool forceNewInstance = false)
//    where T_StateFactory : class, IStateFactory, new()
//  {
//    if (_stateFactory is not T_StateFactory || forceNewInstance)
//    {
//      /// Properly clean previous state cache 
//      /// before resetting the state factory to a new T_StateFactory instance.
//      _stateCache?.CleanAllCache();
//      //_stateFactory?.CleanAllCache();
//      _stateFactory = null;
//    }

    

//    return _stateFactory ??= new T_StateFactory();
//  }

//  /// <summary>
//  /// Get the cached <see cref = "IStateMachineFactory"/> or caching a new <typeparamref name = "T_StateMachineFactory"/> first
//  /// if <see cref = "_stateMachineFactory"/> cached value field is <see langword = "null"/>.
//  /// </summary>
//  /// <typeparam name="T_StateMachineFactory">The actual <see cref="IStateMachineFactory"/> implementing concrete type
//  /// to be cached and returned if not already cached in the first place.</typeparam>
//  /// <param name="forceNewInstance"><see langword="bool"/> flag indicating whether or not 
//  /// reseting to <see langword="null"/> the cached <see cref="_stateMachineFactory"/> field first (default is <see langword="false"/>).</param>
//  /// <returns>The already or to be cached <see cref = "_stateMachineFactory"/> <see cref= "IStateMachineFactory"/> instance.</returns>
//  public static IStateMachineFactory GetStateMachineFactory<T_StateMachineFactory>(bool forceNewInstance = false)
//    where T_StateMachineFactory : class, IStateMachineFactory, new()
//  {
//    if (_stateMachineFactory is not T_StateMachineFactory || forceNewInstance)
//      _stateMachineFactory = null;

//    return _stateMachineFactory ??= new T_StateMachineFactory(); 
//  }

//  /// <summary>
//  /// Get the cached <see cref = "IStateCache"/> or caching a new <typeparamref name = "T_StateCache"/> first
//  /// if <see cref = "_stateCache"/> cached value field is <see langword = "null"/>.
//  /// </summary>
//  /// <typeparam name="T_StateCache">The actual <see cref="IStateCache"/> implementing concrete type
//  /// to be cached and returned if not already cached in the first place.</typeparam>
//  /// <param name="forceNewInstance"><see langword="bool"/> flag indicating whether or not 
//  /// reseting to <see langword="null"/> the cached <see cref="_stateCache"/> field first (default is <see langword="false"/>).</param>
//  /// <returns>The already or to be cached <see cref = "_stateCache"/> <see cref= "IStateCache"/> instance.</returns>
//  public static IStateCache GetStateCache<T_StateCache>(bool forceNewInstance = false)
//    where T_StateCache : class, IStateCache, new()
//  {
//    if (_stateCache is not T_StateCache || forceNewInstance)
//      _stateCache = null;

//    return _stateCache ??= new T_StateCache();
//  }

//  /// <summary>
//  /// Initialize this factories service locator with given <typeparamref name="T_StateFactory"/>, 
//  /// <typeparamref name="T_StateMachineFactory"/> and <typeparamref name="T_StateCache"/> contrete implementing types.
//  /// Used as a cheap replacement of a full Dependency Injection mechanism, whose overhead is not totally justified 
//  /// as it would have concerned only <see cref="IStateFactory"/>, <see cref="IStateMachineFactory"/>, <see cref="IStateCache"/> and <see cref="IStateMachine"/> types here.
//  /// </summary>
//  /// <typeparam name="T_StateFactory">The actual <see cref="IStateFactory"/> implementing concrete type to be used and cached via <see cref="GetStateFactory{T_StateFactory}(bool)"/> method.</typeparam>
//  /// <typeparam name="T_StateMachineFactory">The actual <see cref="IStateMachineFactory"/> implementing concrete type to be used and cached via <see cref="GetStateMachineFactory{T_StateMachineFactory}(bool)"/> method.</typeparam>
//  /// <typeparam name="T_StateCache">The actual <see cref="IStateCache"/> implementing concrete type to be used and cached via <see cref="GetStateCache{T_StateCache}(bool)"/> method.</typeparam>
//  public static void Init<T_StateFactory,T_StateMachineFactory, T_StateCache>()
//    where T_StateFactory : class, IStateFactory, new()
//    where T_StateMachineFactory : class, IStateMachineFactory, new()
//    where T_StateCache : class, IStateCache, new()
//  {
//    GetStateFactory<T_StateFactory>(true);
//    GetStateMachineFactory<T_StateMachineFactory>(true);
//    GetStateCache<T_StateCache>(true);
//  }
//}
