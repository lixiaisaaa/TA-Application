function changeRole(user_id, role, add_remove) {
    $.post(
        {
            url: "/Admin/changeRole",
            data: {
                user_id: user_id,
                role: role,
                add_remove: add_remove
            }
        }).done(function (data) {
            console.log("Sample of data:", data);
        });
}