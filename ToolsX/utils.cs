
using System;
using EnvDTE80;
using Npgsql;
using EnvDTE;
using System.Data;
using System.Windows.Forms;
using System.Xml;
namespace ToolsX
{
    public class utils
    {
        public DTE2 _applicationObject;

        public string GetCurSolution()
        {
            /* 解决方案路径 */
            string strSolutionName = _applicationObject.Solution.FullName;

            /* 提取解决方案名称 */
            int nIndex = strSolutionName.LastIndexOf("\\");
            strSolutionName = strSolutionName.Substring(nIndex + 1, strSolutionName.Length - nIndex - 1);

            return strSolutionName;
        }

        public string GetRelativePath(string strFilePath)
        {
            /* 解决方案路径 */
            string strSolution = _applicationObject.Solution.FullName;
            string[] path1Array = strSolution.ToLower().Split('\\');
            string[] path2Array = strFilePath.ToLower().Split('\\');
            int s = path1Array.Length >= path2Array.Length ? path2Array.Length : path1Array.Length;
            //两个目录最底层的共用目录索引
            int closestRootIndex = -1;
            for (int i = 0; i < s; i++)
            {
                if (path1Array[i] == path2Array[i])
                {
                    closestRootIndex = i;
                }
                else
                {
                    break;
                }
            }
            //由path1计算 ‘../'部分
            string path1Depth = "";
            for (int i = 0; i < path1Array.Length; i++)
            {
                if (i > closestRootIndex + 1)
                {
                    path1Depth += "../";
                }
            }
            //由path2计算 ‘../'后面的目录
            string path2Depth = "";
            for (int i = closestRootIndex + 1; i < path2Array.Length; i++)
            {
                path2Depth += "/" + path2Array[i];
            }
            path2Depth = path2Depth.Substring(1);
            return path1Depth + path2Depth;
        }

        public string GetRealPath(string strRelativePath)
        {
            /* 解决方案路径 */
            string strSolutionName = _applicationObject.Solution.FullName;

            /* 提取解决方案名称 */
            int nIndex = strSolutionName.LastIndexOf("\\");
            string strPath = strSolutionName.Substring(0, nIndex + 1);

            /* 拼接绝对目录 */
            strPath += strRelativePath;

            /* 去除掉 ../../ */
            strPath = System.IO.Path.GetFullPath(strPath);

            return strPath;
        }

    }
}