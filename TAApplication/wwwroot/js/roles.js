function changeRole(user_id, role) {
    $.post(
        {
            url: "/Admin/changeRole",
            data: {
                user_id: user_id,
                role: role
            }
        }).done(function (data) {
            console.log("Sample of data:", data);
        });
}