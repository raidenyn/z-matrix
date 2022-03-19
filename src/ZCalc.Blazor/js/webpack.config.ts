import * as path from 'path'
import HtmlWebpackPlugin from "html-webpack-plugin";

const rootDir = path.resolve(__dirname)
const srcDir = path.resolve(rootDir, 'src')
const outputDir = path.resolve(rootDir, '../wwwroot')

export default (() => ({
    devtool: 'source-map',
    entry: {
        'zcalc': path.resolve(srcDir, 'index.ts'),
    },
    target: ['web', 'es2020'],
    module: {
        rules: [{
            exclude: [/node_modules/],
            test: /\.tsx?$/,
            use: [{
                loader: 'ts-loader',
                options: {
                    configFile: path.resolve(srcDir, 'tsconfig.json'),
                    onlyCompileBundledFiles: true,
                },
            }],
        }],
    },
    output: {
        filename: '[name].[hash].js',
        path: path.resolve(outputDir, 'js'),
        libraryTarget: 'umd',
        library: "zcalc",
    },
    resolve: {
        extensions: ['.ts', '.tsx', '.js'],
    },
    plugins: [
        new HtmlWebpackPlugin({
            template: path.resolve(srcDir, 'index.html'),
            filename:  path.resolve(outputDir, 'index.html'),
            minify: false,
            inject: 'body',
            scriptLoading: 'blocking'
        })
    ]
}))
