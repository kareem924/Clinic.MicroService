namespace Clinic.SharedKernel.Auth
{
    public interface IUserInfo
    {
        /// <summary>
        /// If the current user is logged in
        /// </summary>
        bool IsAuthenticated { get; }

        /// <summary>
        /// Id of the current user if logged in
        /// </summary>
        string Id { get; }

        /// <summary>
        /// UserName of the current user if logged in
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Email of the current user if logged in
        /// </summary>
        string Email { get; }

        /// <summary>
        /// Full Name of the current user if logged in
        /// </summary>
        string FullName { get; }

        bool IsInRoles(params string[] roles);

        bool HasAnyRoles(params string[] roles);
    }
}
