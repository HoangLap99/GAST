$("#home-contact").validate({
  rules: {
        FullName: "required",
        Gender: "required",
        Tel: {
      required: true,
      rangelength: [10, 11],
      number: true,
    },
        Email: {
      required: true,
      email: true,
    },
  },
  messages: {
      FullName: "Please enter Your name",
      Gender: "Please enter Company name",
      Tel: {
      required: "Please enter Your phone",
      rangelength: "Phone has 10 or 11 numbers",
      number: "Please enter numbers",
    },
      Email: {
      required: "Please enter Email",
      email: "Email not true",
    },
  },
});

$("#news-detail").validate({
  rules: {
    name: "required",
    email: {
      required: true,
      email: true,
    },
  },
  messages: {
    name: "Please enter Your name",
    email: {
      required: "Please enter Email",
      email: "Email not true",
    },
  },
});
