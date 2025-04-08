$(document).ready(function () {
    $('.js-delete').on('click', function () {
        var btn = $(this);

        const swal = Swal.mixin({
            customClass: {
                confirmButton: "btn btn-danger ms-3",
                cancelButton: "btn btn-light me-3"
            },
            buttonsStyling: false
        });
        swal.fire({
  title: "Are you sure that you need to delete this game?",
  text: "You won't be able to revert this!",
  icon: "warning",
  showCancelButton: true,
  confirmButtonText: "Yes, delete it!",
  cancelButtonText: "No, cancel!",
  reverseButtons: true
        }).then((result) => {
            console.log(result.isConfirmed);
            if (result.isConfirmed) {

                $.ajax({
                    url: `/Games/Delete/${btn.data('id')}`,
                    method: 'DELETE',
                    success: function () {
                        swal.fire({
                            title: "Deleted!",
                            text: "Game has been deleted.",
                            icon: "success"
                        });
                        btn.parents('tr').fadeOut();
                    },
                    error: function () {
                        swal.fire({
                            title: "Ooops....",
                            text: "Somethings went worng.",
                            icon: "error"
                        });
                    }
                });
   
  } 
});

      
    });
});