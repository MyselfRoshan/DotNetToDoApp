function handleDelete(id) {
  $.ajax({
    url: "Home/Delete",
    type: "POST",
    data: {
      id: id,
    },
    success: function (response) {
      $(`#todo_${response.id}`).remove();
    },
  });
}

function handleEdit(editButton, id) {
  // Add logic for edit button click
  const inputField = $(`#input_${id}`);
  if (inputField.prop("disabled")) {
    inputField.prop("disabled", false);
    editButton.innerHTML = EDIT_ICONS.on;
  } else {
    $.ajax({
      url: "Home/Update",
      type: "PATCH",
      data: {
        id: id,
        title: inputField.val(),
      },
      dataType: "json",
      success: function (response) {
        console.log(response);
        inputField.val(response.title);
      },
    });
    inputField.prop("disabled", true);
    editButton.innerHTML = EDIT_ICONS.off;
  }
  console.log(editButton, id);
}

const EDIT_ICONS = {
  off: `<svg xmlns="http://www.w3.org/2000/svg" width="16" height="16" fill="currentColor" class="prefix__bi prefix__bi-pencil-square"><path d="M15.502 1.94a.5.5 0 010 .706L14.459 3.69l-2-2L13.502.646a.5.5 0 01.707 0l1.293 1.293zm-1.75 2.456l-2-2L4.939 9.21a.5.5 0 00-.121.196l-.805 2.414a.25.25 0 00.316.316l2.414-.805a.5.5 0 00.196-.12l6.813-6.814z"/><path fill-rule="evenodd" d="M1 13.5A1.5 1.5 0 002.5 15h11a1.5 1.5 0 001.5-1.5v-6a.5.5 0 00-1 0v6a.5.5 0 01-.5.5h-11a.5.5 0 01-.5-.5v-11a.5.5 0 01.5-.5H9a.5.5 0 000-1H2.5A1.5 1.5 0 001 2.5z"/></svg>`,
  on: `<svg xmlns="http://www.w3.org/2000/svg" width="16" height="16" fill="currentColor" class="prefix__bi prefix__bi-check"><path d="M10.97 4.97a.75.75 0 011.07 1.05l-3.99 4.99a.75.75 0 01-1.08.02L4.324 8.384a.75.75 0 111.06-1.06l2.094 2.093 3.473-4.425z"/></svg>`,
};
