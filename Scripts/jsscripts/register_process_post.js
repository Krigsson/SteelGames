function test() {
    $("#registerLink").click(function () {

        var email = $("#email").val();
        var password = $("#password").val();
        var phone = $("#phone").val();

        var form = $("<form>").attr({
            method: "post",
            action: "@Url.Action("Register", "Account")" // Replace with your action and controller names
                });

        // Append any form data as needed
        form.append($("<input>").attr({ type: "hidden", name: "email", value: email }));
        form.append($("<input>").attr({ type: "hidden", name: "password", value: password }));
        form.append($("<input>").attr({ type: "hidden", name: "phone", value: phone }));

        // Append form to document and submit it
        $("body").append(form);
        form.submit();
    });
}