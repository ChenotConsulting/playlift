/// <reference path="../../../Scripts/jquery-1.8.2.js" />

function loadPasswordChangeForm() {
    var oldPassword = $("#OldPassword"),
      newPassword = $("#NewPassword"),
      confirmPassword = $("#ConfirmPassword"),
      allFields = $([]).add(oldPassword).add(newPassword).add(confirmPassword),
      tips = $(".validateTips");

    function updateTips(t) {
        tips
          .text(t)
          .addClass("ui-state-highlight");
        setTimeout(function () {
            tips.removeClass("ui-state-highlight", 1500);
        }, 500);
    }

    function checkLength(o, n, min, max) {
        if (o.val().length > max || o.val().length < min) {
            o.addClass("ui-state-error");
            updateTips("Length of " + n + " must be between " +
              min + " and " + max + ".");
            return false;
        } else {
            return true;
        }
    }

    function checkPasswordMatch(o, n) {
        if (o.val() != n.val()) {
            o.addClass("ui-state-error");
            n.addClass("ui-state-error");
            updateTips("The passwords must match.");
            return false;
        } else {
            return true;
        }
    }

    function checkRegexp(o, regexp, n) {
        if (!(regexp.test(o.val()))) {
            o.addClass("ui-state-error");
            updateTips(n);
            return false;
        } else {
            return true;
        }
    }

    function savePassword() {
        // validate the form before submitting it
        var bValid = true;
        var formCollection = $("#passwordChange").serialize();

        allFields.removeClass("ui-state-error");

        bValid = bValid && checkLength(oldPassword, "old password", 6, 128);
        bValid = bValid && checkLength(newPassword, "new password", 6, 128);
        bValid = bValid && checkLength(confirmPassword, "confirm password", 6, 128);
        bValid = bValid && checkPasswordMatch(newPassword, confirmPassword);

        if (bValid) {
            var data = { form: formCollection };

            $.ajax({
                cache: false,
                type: 'POST',
                async: true,
                dataType: "html",
                data: data,
                url: '/Account/PasswordChange',
                data: data,
                success: function (html) {
                    alert(html);
                    loadView('profilePlaceholder', '/Account/PasswordChange/');
                },
                error: function (xhr) {
                    alert(xhr.statusText);
                }
            });
        }
        else {
            $(".ui-state-error").first().focus();
        }
    }

    $("#save-password").click(function (event) {
        savePassword();
        event.preventDefault();
    });
}