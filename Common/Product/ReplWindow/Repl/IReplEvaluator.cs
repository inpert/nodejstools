﻿//*********************************************************//
//    Copyright (c) Microsoft. All rights reserved.
//    
//    Apache 2.0 License
//    
//    You may obtain a copy of the License at
//    http://www.apache.org/licenses/LICENSE-2.0
//    
//    Unless required by applicable law or agreed to in writing, software 
//    distributed under the License is distributed on an "AS IS" BASIS, 
//    WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or 
//    implied. See the License for the specific language governing 
//    permissions and limitations under the License.
//
//*********************************************************//

using System;
using System.Threading.Tasks;
using Microsoft.VisualStudio.Text;

#if NTVS_FEATURE_INTERACTIVEWINDOW
namespace Microsoft.NodejsTools.Repl {
#else
namespace Microsoft.VisualStudio.Repl {
#endif
#if INTERACTIVE_WINDOW
    using IReplWindow = IInteractiveWindow;
#endif

    /// <summary>
    /// Implements an evaluator for a specific REPL implementation.  The evaluator is provided to the
    /// REPL implementation by the IReplEvaluatorProvider interface.
    /// </summary>
#if INTERACTIVE_WINDOW
    public interface IInteractiveEngine : IDisposable {
#else
    public interface IReplEvaluator : IDisposable {
#endif
        /// <summary>
        /// Initializes the interactive session. 
        /// </summary>
        /// <param name="window">Window the evaluator is initialized for.</param>
        /// <returns>Task that completes the initialization.</returns>
        /// <remarks>
        /// Usually creates a new process locally or remotely and sets up communication that's necessary.
        /// 
        /// Called after the text view has been created and the repl window is about to start. The implementors
        /// might want to store the <paramref name="window"/> and adjust its look and behavior.
        /// </remarks>
        Task<ExecutionResult> Initialize(IReplWindow window);

        /// <summary>
        /// Invoked when the active language buffer changes.
        /// </summary>
        /// <param name="currentBuffer">
        /// The new buffer added to the sequence of buffers that contain code submissions. 
        /// This buffer is now the active buffer for code input.
        /// </param>
        /// <param name="previousBuffer">
        /// The buffer that contains just submitted code. 
        /// Null if the <paramref name="currentBuffer"/> is the first buffer in the sequence of language buffers.
        /// </param>
        void ActiveLanguageBufferChanged(ITextBuffer currentBuffer, ITextBuffer previousBuffer);

        /// <summary>
        /// Re-starts the interpreter. Usually this closes the current process (if alive) and starts
        /// a new interpreter.
        /// </summary>
        /// <returns>Task that completes reset and initialization of the new process.</returns>
        Task<ExecutionResult> Reset();

        // Parsing and Execution

        /// <summary>
        /// Returns true if the text can be executed.  Used to determine if there is a whole statement entered
        /// in the REPL window.
        /// </summary>
        bool CanExecuteText(string/*!*/ text);

        /// <summary>
        /// Asynchronously executes the specified text.
        /// </summary>
        /// <param name="text">The code snippet to execute.</param>
        /// <returns>Task that completes the execution.</returns>
        Task<ExecutionResult> ExecuteText(string text);
        
        void ExecuteFile(string filename);

        /// <summary>
        /// Formats the contents of the clipboard in a manner reasonable for the language.  Returns null if the
        /// current clipboard cannot be formatted.
        /// 
        /// By default if the clipboard contains text it will be pasted.  The language can format additional forms
        /// here - for example CSV data can be formatted in a languaeg compatible manner.
        /// </summary>
        string FormatClipboard();

        /// <summary>
        /// Aborts the current running command.
        /// </summary>
        void AbortCommand();
    }
}
