using System;
using System.Collections;
using System.Collections.Generic;

namespace WumpWump.Net.Entities
{
    public partial struct DiscordPermissionContainer : IEnumerable<DiscordPermission>
    {
        /// <summary>
        /// An enumerator for the <see cref="DiscordPermissionContainer"/>. This will enumerate
        /// all the permissions that are set to true in the container.
        /// </summary>
        public struct DiscordPermissionContainerEnumerator : IEnumerator<DiscordPermission>
        {
            /// <summary>
            /// When true, the current permission is documented. This means that it is a valid
            /// permission and not just a random bit in the container. Alternatively, if the current
            /// version of the library is outdated, this may return false for a documented permission
            /// that was not documented at the time of release.
            /// </summary>
            /// <returns>Whether or not the permission is documented at compile-time.</returns>
            public readonly bool IsDocumented => Enum.IsDefined(Current);

            /// <inheritdoc cref="IEnumerator{T}.Current"/>
            public readonly DiscordPermission Current => (DiscordPermission)_currentBit;
            readonly object IEnumerator.Current => Current;

            /// <summary>
            /// A copy of the <see cref="DiscordPermissionContainer"/> that is being enumerated.
            /// </summary>
            private readonly DiscordPermissionContainer _permissions;

            /// <summary>
            /// The current bit that is being enumerated. This is the index of the bit in the <see cref="DiscordPermissionContainer"/>.
            /// </summary>
            private int _currentBit = -1;

            /// <summary>
            /// Creates a new <see cref="DiscordPermissionContainerEnumerator"/> for the specified <see cref="DiscordPermissionContainer"/>.
            /// </summary>
            /// <param name="permissions">The <see cref="DiscordPermissionContainer"/> to enumerate.</param>
            public DiscordPermissionContainerEnumerator(DiscordPermissionContainer permissions) => _permissions = new(permissions);

            /// <inheritdoc cref="IEnumerator{T}.MoveNext"/>
            public bool MoveNext()
            {
                while (_currentBit < MAXIMUM_BIT_COUNT - 1)
                {
                    if (_permissions.HasFlag(++_currentBit))
                    {
                        return true;
                    }
                }

                return false;
            }

            /// <inheritdoc cref="IEnumerator{T}.Reset"/>
            public void Reset() => _currentBit = -1;

            /// <inheritdoc cref="IDisposable.Dispose"/>
            public readonly void Dispose() { }
        }

        /// <inheritdoc cref="IEnumerable{T}.GetEnumerator"/>
        public IEnumerator<DiscordPermission> GetEnumerator() => new DiscordPermissionContainerEnumerator(this);
        IEnumerator IEnumerable.GetEnumerator() => new DiscordPermissionContainerEnumerator(this);
    }
}