/** @type {import('tailwindcss').Config} */
module.exports = {
    mode: 'jit',
    content: [
        "./Components/**/*.{html,razor}",
        "./wwwroot/**/*.{html,htm}",
    ],
    theme: {
      extend: {
      },
    },
    plugins: [],
    prefix: 'tw-'
}
