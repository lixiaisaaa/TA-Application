function changeRole(ID, Role) {
    $.post(
        {
            url: "/Admin/changeRole",
            data: {
                ID: ID,
                Role: Role
            }
        }).done(function (data) {
            console.log("Sample of data:", data);
        });
}