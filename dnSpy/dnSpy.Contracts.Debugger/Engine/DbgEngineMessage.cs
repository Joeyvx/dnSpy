﻿/*
    Copyright (C) 2014-2017 de4dot@gmail.com

    This file is part of dnSpy

    dnSpy is free software: you can redistribute it and/or modify
    it under the terms of the GNU General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    (at your option) any later version.

    dnSpy is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU General Public License for more details.

    You should have received a copy of the GNU General Public License
    along with dnSpy.  If not, see <http://www.gnu.org/licenses/>.
*/

using System;
using dnSpy.Contracts.Debugger.Exceptions;

namespace dnSpy.Contracts.Debugger.Engine {
	/// <summary>
	/// Base class of messages created by a <see cref="DbgEngine"/>
	/// </summary>
	public abstract class DbgEngineMessage {
		/// <summary>
		/// Gets the message kind
		/// </summary>
		public abstract DbgEngineMessageKind MessageKind { get; }
	}

	/// <summary>
	/// Base class of messages created by a <see cref="DbgEngine"/> that can contain an error message
	/// </summary>
	public abstract class DbgEngineMessageWithPossibleErrorMessage : DbgEngineMessage {
		/// <summary>
		/// The error message or null if there's no error
		/// </summary>
		public string ErrorMessage { get; }

		/// <summary>
		/// Constructor
		/// </summary>
		protected DbgEngineMessageWithPossibleErrorMessage() { }

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="errorMessage">Error message</param>
		protected DbgEngineMessageWithPossibleErrorMessage(string errorMessage) => ErrorMessage = errorMessage ?? throw new ArgumentNullException(nameof(errorMessage));
	}

	/// <summary>
	/// <see cref="DbgEngineMessageKind.Connected"/> event. Should be the first event sent by the
	/// debug engine. If it couldn't connect, no more messages need to be sent after this message
	/// is sent.
	/// </summary>
	public sealed class DbgMessageConnected : DbgEngineMessageWithPossibleErrorMessage {
		/// <summary>
		/// Returns <see cref="DbgEngineMessageKind.Connected"/>
		/// </summary>
		public override DbgEngineMessageKind MessageKind => DbgEngineMessageKind.Connected;

		/// <summary>
		/// Gets the process id
		/// </summary>
		public int ProcessId { get; }

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="processId">Process id</param>
		public DbgMessageConnected(int processId) => ProcessId = processId;

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="errorMessage">Error message</param>
		public DbgMessageConnected(string errorMessage) : base(errorMessage) { }
	}

	/// <summary>
	/// <see cref="DbgEngineMessageKind.Disconnected"/> event
	/// </summary>
	public sealed class DbgMessageDisconnected : DbgEngineMessage {
		/// <summary>
		/// Returns <see cref="DbgEngineMessageKind.Disconnected"/>
		/// </summary>
		public override DbgEngineMessageKind MessageKind => DbgEngineMessageKind.Disconnected;

		/// <summary>
		/// Gets the exit code
		/// </summary>
		public int ExitCode { get; }

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="exitCode">Exit code</param>
		public DbgMessageDisconnected(int exitCode) => ExitCode = exitCode;
	}

	/// <summary>
	/// <see cref="DbgEngineMessageKind.Break"/> event
	/// </summary>
	public sealed class DbgMessageBreak : DbgEngineMessageWithPossibleErrorMessage {
		/// <summary>
		/// Returns <see cref="DbgEngineMessageKind.Break"/>
		/// </summary>
		public override DbgEngineMessageKind MessageKind => DbgEngineMessageKind.Break;

		/// <summary>
		/// Constructor
		/// </summary>
		public DbgMessageBreak() { }

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="errorMessage">Error message</param>
		public DbgMessageBreak(string errorMessage) : base(errorMessage) { }
	}

	/// <summary>
	/// <see cref="DbgEngineMessageKind.Exception"/> event
	/// </summary>
	public sealed class DbgMessageException : DbgEngineMessageWithPossibleErrorMessage {
		/// <summary>
		/// Returns <see cref="DbgEngineMessageKind.Exception"/>
		/// </summary>
		public override DbgEngineMessageKind MessageKind => DbgEngineMessageKind.Exception;

		/// <summary>
		/// Gets the exception
		/// </summary>
		public DbgException Exception { get; }

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="exception">Exception</param>
		public DbgMessageException(DbgException exception) =>
			Exception = exception ?? throw new ArgumentNullException(nameof(exception));
	}
}