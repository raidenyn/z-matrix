module.exports = {
    "env": {
        "browser": true,
        "node": false
    },
    "extends": [
        "plugin:@typescript-eslint/recommended",
        "plugin:@typescript-eslint/recommended-requiring-type-checking",
        "prettier"
    ],
    "parser": "@typescript-eslint/parser",
    "parserOptions": {
        "project": __dirname + "/src/@agoda/tsconfig.json",
        "sourceType": "module"
    },
    "plugins": [
        "eslint-plugin-prefer-arrow",
        "eslint-plugin-jsdoc",
        "@typescript-eslint"
    ]
};
