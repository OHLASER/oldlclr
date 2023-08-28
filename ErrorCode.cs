namespace oldlclr
{
    /// <summary>
    /// data link protocol error code
    /// </summary>
    public enum ErrorCode
    {
        /// <summary>
        /// no error code
        /// </summary>
        NO_ERROR = 0,
        /// <summary>
        /// out of memory
        /// </summary>
        OUT_OF_MEMORY = -1,
        /// <summary>
        /// invalidate arguments.
        /// </summary>
        INVALID_ARGS = -2,
        /// <summary>
        /// can not allocate file descriptor.
        /// </summary>
        CANT_ALLOCATE_FILE_DESC = -3,
        /// <summary>
        /// file descriptor is closed
        /// </summary>
        FILE_DESC_CLOSED = -4,
        /// <summary>
        /// wrong command format.
        /// </summary>
        WRONG_COMMAND_FORMAT = -5,
        /// <summary>
        /// can not generate unique id.
        /// </summary>
        CANT_GENERATE_UNIQUE_ID = -6,
        /// <summary>
        /// command is not acceptable.
        /// </summary>
        CMD_NOT_ACCEPABLE = -7,
        /// <summary>
        /// not communicated.
        /// </summary>
        NOT_COMMUNICATE = -8,
        /// <summary>
        /// unexpected status
        /// </summary>
        UNEXPECTED_STATUS = -9,
        /// <summary>
        /// receiver is not attached
        /// </summary>
        RECEIVER_IS_NOT_ATTACHED = -10,
        /// <summary>
        /// not accept duplicated command.
        /// </summary>
        NOT_ACCEPT_DUPLICATE_COMMAND = -11,
        /// <summary>
        /// invalid data format.
        /// </summary>
        INVALID_DATA_FORMAT = -12,
        /// <summary>
        /// receiver is busy.
        /// </summary>
        RECEIVER_IS_BUSY = -13
    }
}
